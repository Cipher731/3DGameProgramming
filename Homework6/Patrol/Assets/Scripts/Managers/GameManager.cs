using System;
using System.Collections;
using Patrol;
using Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private GameObject _player;

        private int _level = 1;
        private BoardManager _boardManager;
        private ScoreManager _scoreManager;

        private bool _isGameover;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(Instance);
            }

            DontDestroyOnLoad(gameObject);
            _boardManager = GetComponent<BoardManager>();
            _boardManager.FactoryManager = GetComponent<PatrolFactoryManager>();

            _scoreManager = GetComponent<ScoreManager>();
            
            _player = GameObject.FindWithTag("Player");
            DontDestroyOnLoad(_player);

            PlayerInteraction.LevelUp += OnLevelUp;
            PatrolAi.CaughtTarget += Gameover;

            StartCoroutine(BlockPlayerInitGame());
        }

        private IEnumerator BlockPlayerInitGame()
        {
            _player.SetActive(false);
            InitGame();
            yield return new WaitForSeconds(4f);
            _player.SetActive(true);
        }

        private void InitGame()
        {
            _isGameover = false;
            _player.transform.position = new Vector3(20, 0, 13);
            _player.transform.rotation = Quaternion.Euler(0, -128, 0);
            _player.GetComponent<PlayerMovement>().enabled = true;
            _boardManager.SetupScene(_level);
        }

        private IEnumerator LevelUpCoroutine()
        {
            _player.SetActive(false);
            yield return new WaitForSeconds(1f);
            InitGame();
            yield return new WaitForSeconds(2f);
            _player.SetActive(true);
        }

        private void OnLevelUp(object sender, EventArgs args)
        {
            _level++;
            
            var guiManager = GuiManager.Instance;
            guiManager.LevelText.text = "Level: " + _level;
            guiManager.ShowLevel();

            StartCoroutine(LevelUpCoroutine());
        }

        private IEnumerator LoadGameCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            _player.SetActive(false);
            yield return new WaitForSeconds(3.5f);

            _level = 1;
            _scoreManager.Score = 0;
            InitGame();
            
            _player.GetComponent<Animator>().SetTrigger("Restart");
            GuiManager.Instance.Restart();
            
            yield return new WaitForSeconds(4f);
            _player.SetActive(true);
        }

        private void Gameover(object o, EventArgs eventArgs)
        {
            if (_isGameover)
            {
                return;
            }

            _isGameover = true;
            var guiManager = GuiManager.Instance;
            guiManager.GameoverText.text = "YOU ARE DOOMED!!!\n";
            guiManager.ShowGameover();
            StartCoroutine(LoadGameCoroutine());
        }
    }
}