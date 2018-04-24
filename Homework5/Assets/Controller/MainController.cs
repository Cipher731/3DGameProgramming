using Interface;
using UnityEngine;

namespace Controller
{
    public class MainController : MonoBehaviour, ISceneController
    {
        public GameObject GameController;

        private void Awake()
        {
            Director.Instance.CurrentSceneController = this;
            Director.Instance.SetFps(60);
            LoadGame();
        }

        public void LoadGame()
        {
            if (Controller.GameController.Instance == null)
            {
                Instantiate(GameController);
            }
        }
    }
}