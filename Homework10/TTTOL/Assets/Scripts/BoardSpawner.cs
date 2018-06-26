using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BoardSpawner : NetworkBehaviour
{
    public GameObject BoardPrefab;
    public GameObject GridSpacePrefab;

    private readonly List<GameObject> _gridSpaces = new List<GameObject>();
    private string _mark;
    private string _turn;
    private int _moves;

    private readonly int[][] _winConditions =
    {
        new[] {0, 1, 2},
        new[] {3, 4, 5},
        new[] {6, 7, 8},
        new[] {0, 3, 6},
        new[] {1, 4, 7},
        new[] {2, 5, 8},
        new[] {0, 4, 8},
        new[] {2, 4, 6}
    };

    public void Start()
    {
        _turn = "O";
        _mark = isServer ? "O" : "X";
        var board = Instantiate(BoardPrefab);
        for (var i = 0; i <= 2; i++)
        {
            for (var j = 0; j <= 2; j++)
            {
                var index = i * 3 + j;
                var gridSpace = Instantiate(GridSpacePrefab);
                gridSpace.transform.SetParent(board.transform);
                gridSpace.transform.localPosition = new Vector3(170 * j - 170, -170 * i + 170);
                gridSpace.GetComponent<Button>().onClick.AddListener(() => CmdMarkSquare(index, _mark));
                _gridSpaces.Add(gridSpace);
            }
        }

        board.transform.SetParent(transform);
    }

    [Command]
    private void CmdMarkSquare(int index, string mark)
    {
        if (!_turn.Equals(mark) || NetworkServer.connections.Count < 2 ||
            !_gridSpaces[index].GetComponentInChildren<Text>().text.Equals(""))
        {
            return;
        }

        RpcMarkSquare(index, mark);
        _turn = _turn.Equals("X") ? "O" : "X";
    }

    [ClientRpc]
    private void RpcMarkSquare(int index, string mark)
    {
        _moves++;
        _gridSpaces[index].GetComponentInChildren<Text>().text = mark;
        CheckGame();
    }

    private void CheckGame()
    {
        if (!isServer)
        {
            return;
        }

        var grid = _gridSpaces.Select(e => e.GetComponentInChildren<Text>().text).ToArray();
        foreach (var win in _winConditions)
        {
            if (!grid[win[0]].Equals("") && grid[win[0]].Equals(grid[win[1]]) && grid[win[0]].Equals(grid[win[2]]))
            {
                RpcGameOver(grid[win[0]]);
                return;
            }
        }

        if (_moves >= 9)
        {
            RpcGameOver("Cat");
        }
    }

    [ClientRpc]
    private void RpcGameOver(string mark)
    {
        var text = GameObject.FindGameObjectsWithTag("GameOverText").Select(e => e.GetComponent<Text>());
        foreach (var t in text)
        {
            t.text = mark + " win!";
        }
    }
}