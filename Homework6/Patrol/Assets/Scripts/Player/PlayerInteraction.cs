using System;
using Patrol;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private Animator _anim;
        public static event EventHandler LevelUp;

        private bool _isDead;
        
        private void Awake()
        {
            _anim = GetComponent<Animator>();

            PatrolAi.CaughtTarget += PlayerDead;
        }

        private void PlayerDead(object sender, EventArgs args)
        {
            if (_isDead)
            {
                return;
            }
            
            _anim.SetTrigger("Dead");
            _isDead = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Exit"))
            {
                if (LevelUp != null)
                {
                    LevelUp(this, EventArgs.Empty);
                }
            }
        }
    }
}