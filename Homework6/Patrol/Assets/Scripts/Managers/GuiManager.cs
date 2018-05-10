using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GuiManager : MonoBehaviour
    {
        public static GuiManager Instance;

        public Text ScoreText;
        public Text GameoverText;
        public Text LevelText;
        public Image Background;

        private Animator _anim;

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
            _anim = GetComponent<Animator>();
        }

        public void ShowLevel()
        {
            _anim.SetTrigger("LevelUp");
        }

        public void ShowGameover()
        {
            Background.transform.SetAsFirstSibling();
            _anim.SetTrigger("Gameover");
        }

        public void Restart()
        {
            ScoreText.transform.SetAsFirstSibling();
            LevelText.text = "Welcome to the game!!!\nTry to get rid of enimies and\ngo to next level to get points.";
            _anim.SetTrigger("Restart");
        }
    }
}