using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance; // Provide instance to other sciprts.
        private WaveController _waveController;
        public ActionManager ActionManager;
        public PhysicsActionManager PhysicsActionManager;

        public int Score;
        public int TotalRound;
        public int WavesPerRound;

        public int CurrentRound;
        public int CurrentWave;

        private const float WaveWait = 3f;

        private Transform _plateHolder;

        private void Awake()
        {
            // Make sure there is only ONE game manager.
            if (Instance == null)
            {
                Instance = this;
            }
            // If there is already one manager, destroy the current one.
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            // Avoid destrution of manager when reloading scene.
            DontDestroyOnLoad(gameObject);

            // Get the reference of RoundManager script.
            _waveController = GetComponent<WaveController>();
            ActionManager = GetComponent<ActionManager>();
            PhysicsActionManager = GetComponent<PhysicsActionManager>();

            StartCoroutine(SpawnPlates());
        }

        private IEnumerator SpawnPlates()
        {
            while (CurrentRound < TotalRound)
            {
                CurrentWave = 0;
                while (CurrentWave < WavesPerRound)
                {
                    _waveController.EmitPlates();
                    yield return new WaitUntil(_waveController.IsIdle);
                    yield return new WaitForSeconds(WaveWait);
                    CurrentWave++;
                }

                CurrentRound++;
            }
        }

        public void ReloadGame()
        {
            StopAllCoroutines();
            _waveController.StopAllCoroutines();

            CurrentRound = 0;
            CurrentWave = 0;
            Score = 0;

            SceneManager.LoadScene(0);
            StartCoroutine(SpawnPlates());
        }
    }
}