using System.Text;
using Controller;
using UnityEngine;

namespace Util
{
    public class GameGui : MonoBehaviour
    {
        private void OnGUI()
        {
            var controller = GameController.Instance;
            if (controller.CurrentRound >= controller.TotalRound)
            {
                GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 120, 50),
                    "You score is " + controller.Score);
                if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 25, 100, 20), "Restart"))
                {
                    controller.ReloadGame();
                }
                return;
            }

            var text = new StringBuilder();

            text.Append("Score: ");
            text.Append(controller.Score);
            text.Append("\n");

            text.Append("Round: ");
            text.Append(controller.CurrentRound + 1);
            text.Append("/");
            text.Append(controller.TotalRound);
            text.Append("\n");

            text.Append("Wave: ");
            text.Append(controller.CurrentWave + 1);
            text.Append("/");
            text.Append(controller.WavesPerRound);

            GUI.Box(new Rect(10, 10, 100, 90), text.ToString());

            if (GUI.Button(new Rect(20, 70, 80, 20), "Restart"))
            {
                controller.ReloadGame();
            }
        }
    }
}