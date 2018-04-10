using Interface;
using UnityEngine;

namespace Util
{
    public enum GameStat
    {
        Win,
        Fail,
        OnGoing
    }

    public class GameGui : MonoBehaviour
    {
        private IUserAction _action;
        private GUIStyle _buttonStyle;
        private GameStat _state = GameStat.OnGoing;

        private GUIStyle _textStyle;

        private void Start()
        {
            _action = Director.GetInstance().CurrentSceneController as IUserAction;

            _textStyle = new GUIStyle
            {
                fontSize = 40,
                alignment = TextAnchor.MiddleCenter
            };

            _buttonStyle = new GUIStyle {fontSize = 30};
        }

        private void OnGUI()
        {
            if (_state == GameStat.Win)
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 50), "Congratulations!",
                    _textStyle);
                // ReSharper disable once InvertIf
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2f, 140, 70), "Restart", _buttonStyle))
                {
                    _state = GameStat.OnGoing;
                    _action.StartGame();
                }
            }
            else if (_state == GameStat.Fail)
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 50), "You failed.", _textStyle);
                // ReSharper disable once InvertIf
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2f, 140, 70), "Restart", _buttonStyle))
                {
                    _state = GameStat.OnGoing;
                    _action.StartGame();
                }
            }
        }

        public void Win()
        {
            _state = GameStat.Win;
        }

        public void Fail()
        {
            _state = GameStat.Fail;
        }
    }
}