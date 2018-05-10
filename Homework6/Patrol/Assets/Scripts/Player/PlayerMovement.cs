using Patrol;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed = 6f;
        private Vector3 _movement;
        private Animator _anim;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();

            PatrolAi.CaughtTarget += (sender, args) => { enabled = false; };
        }

        private void FixedUpdate()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            Move(h, v);
            Turning();
            Animating(h, v);
        }

        private void Move(float h, float v)
        {
            _movement.Set(h, 0f, v);
            _movement = _movement.normalized * Speed * Time.deltaTime;
            _rigidbody.MovePosition(transform.position + _movement);
        }

        private void Turning()
        {
            if (_movement == Vector3.zero)
            {
                return;
            }

            var targetRotation = Quaternion.LookRotation(_movement);
            var deltaRotation = Quaternion.Lerp(_rigidbody.rotation, targetRotation, 5f * Time.deltaTime);
            _rigidbody.MoveRotation(deltaRotation);
        }

        private void Animating(float h, float v)
        {
            var walking = h != 0 || v != 0;
            _anim.SetBool("IsWalking", walking);
        }
    }
}