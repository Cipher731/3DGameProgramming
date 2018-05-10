using System;
using Patrol;
using Player;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int Score;

        private const int PointsPerLostTarget = 1;
        private const int PointsPerLevelUp = 5;

        private void Awake()
        {
            PatrolAi.LostTarget += ScorePoints(PointsPerLostTarget);
            PlayerInteraction.LevelUp += ScorePoints(PointsPerLevelUp);
            
        }

        private EventHandler ScorePoints(int points)
        {
            return (sender, args) => { Score += points; };
        }

        private void Update()
        {
            GuiManager.Instance.ScoreText.text = "Score: " + Score;
        }
    }
}