using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    private const string Nought = "○";
    private const string Cross = "x";

    private string[] _grid;
    private bool _isGameOver;
    private string _gameOverText;
    private int _moves;
    private string _playerSide;

    private void ChangeSides()
    {
        _playerSide = _playerSide == Nought ? Cross : Nought;
    }

    private void EndTurn()
    {
        var winConditions = new[]
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

        foreach (var winCondition in winConditions)
            if (winCondition.All(pos => _grid[pos] == _playerSide))
                GameOver();

        if (++_moves >= 9)
            GameOver();
        else
            ChangeSides();
    }

    private void GameOver()
    {
        _isGameOver = true;
        _gameOverText = _moves >= 9 ? "This is a draw." : _playerSide + " wins!";
    }

    // Use this for initialization
    private void Start()
    {
        _grid = new string[9];
        _playerSide = Cross;
        _isGameOver = false;
        _moves = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnGUI()
    {
        if (_isGameOver)
        {
            GUI.skin.button.fontSize = 15;
            GUI.skin.box.fontSize = 20;

            GUI.Box(new Rect(10, 10, 300, 80), _gameOverText);
            if (GUI.Button(new Rect(110, 50, 100, 20), "Restart"))
                Start();
        }
        else
        {
            GUI.skin.button.fontSize = 30;

            for (var row = 0; row < 3; row++)
                for (var col = 0; col < 3; col++)
                {
                    if (!GUI.Button(new Rect(10 + 110 * col, 10 + 110 * row, 100, 100), _grid[3 * row + col])) continue;

                    _grid[3 * row + col] = _playerSide;
                    EndTurn();
                }
        }
    }
}