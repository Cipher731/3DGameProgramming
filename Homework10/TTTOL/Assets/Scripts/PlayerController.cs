//using UnityEngine.Networking;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class PlayerController : NetworkBehaviour
//{   
//    private string _mark;
//    private string _turn;
//    private int _moves;
//    private List<GameObject> _gridSpaces;
//    
//    private readonly int[][] _winConditions =
//    {
//        new[] {0, 1, 2},
//        new[] {3, 4, 5},
//        new[] {6, 7, 8},
//        new[] {0, 3, 6},
//        new[] {1, 4, 7},
//        new[] {2, 5, 8},
//        new[] {0, 4, 8},
//        new[] {2, 4, 6}
//    };
//
//    public override void OnStartLocalPlayer()
//    {
//        _gridSpaces = GameObject.Find("GameBoard").GetComponent<BoardSpawner>().GridSpaces;
//        
//        _turn = "O";
//        _mark = isServer ? "O" : "X";
//        
//        for (var i = 0; i < 9; i++)
//        {
//            var index = i;
//            _gridSpaces[i].GetComponent<Button>().onClick.AddListener(() => CmdMarkSquare(index, _mark));
//        }
//    }
//
//    [Command]
//    private void CmdMarkSquare(int index, string mark)
//    {
//        if (!_turn.Equals(mark) || NetworkServer.connections.Count < 2 ||
//            !_gridSpaces[index].GetComponentInChildren<Text>().text.Equals(""))
//        {
//            return;
//        }
//
//        RpcMarkSquare(index, mark);
//        _turn = _turn.Equals("X") ? "O" : "X";
//    }
//
//    [ClientRpc]
//    private void RpcMarkSquare(int index, string mark)
//    {
//        if (!isLocalPlayer)
//        {
//            return;
//        }
//        _moves++;
//        _gridSpaces[index].GetComponentInChildren<Text>().text = mark;
//        CheckGame();
//    }
//
//    private void CheckGame()
//    {
//        if (!isServer)
//        {
//            return;
//        }
//
//        var grid = _gridSpaces.Select(e => e.GetComponentInChildren<Text>().text).ToArray();
//        foreach (var win in _winConditions)
//        {
//            if (!grid[win[0]].Equals("") && grid[win[0]].Equals(grid[win[1]]) && grid[win[0]].Equals(grid[win[2]]))
//            {
//                RpcGameOver(grid[win[0]]);
//                return;
//            }
//        }
//
//        if (_moves >= 9)
//        {
//            RpcGameOver("Cat");
//        }
//    }
//
//    [ClientRpc]
//    private void RpcGameOver(string mark)
//    {
//        var text = GameObject.FindGameObjectWithTag("GameOverText");
//        text.GetComponent<Text>().text = mark + " wins";
//    }
//}