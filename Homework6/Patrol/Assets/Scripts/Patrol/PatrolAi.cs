using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Patrol
{
    public class PatrolAi : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Transform _target;
        private Animator _anim;

        private bool _patrolling;

        private const float ReselectDelay = 5;
        private float _reselectTimer;

        private const float WaitDelay = 1;
        private float _waitTimer;

        public static event EventHandler LostTarget;
        public static event EventHandler CaughtTarget;

        public float ChasingSpeed = 3.5f;
        public float PatrollingSpeed = 2f;
        
        private void Awake()
        {
            _patrolling = true;
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _anim.SetBool("IsWalking", true);

            SceneManager.sceneLoaded += ResetStatus;
            
            SelectPatrolDirection();
        }

        private void FixedUpdate()
        {
            if (_patrolling)
            {
                Patrol();
            }
            else
            {
                _agent.isStopped = false;
                if (_target != null)
                {
                    _agent.SetDestination(_target.position);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                StartChasing(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                StopChasing();

                if (LostTarget != null)
                {
                    LostTarget(this, EventArgs.Empty);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Player"))
            {
                StopChasing();
                
                if (CaughtTarget != null)
                {
                    CaughtTarget(this, EventArgs.Empty);
                }
            }
            else
            {
                TurnBack();
            }
        }

        private void StartChasing(Transform target)
        {
            _agent.speed = ChasingSpeed;
            _patrolling = false;
            _anim.SetBool("IsWalking", true);
            _target = target;
        }

        private void StopChasing()
        {
            _patrolling = true;
            _target = null;
            _agent.isStopped = true;
            _anim.SetBool("IsWalking", false);
            SelectPatrolDirection();
        }

        public void InitializeState()
        {
            _patrolling = true;
            _target = null;
            _agent.isStopped = true;
            _anim.SetBool("IsWalking", true);
            SelectPatrolDirection();
        }

        private void Patrol()
        {
            if (_reselectTimer <= ReselectDelay)
            {
                _reselectTimer += Time.deltaTime;
            }
            else if (_waitTimer <= WaitDelay)
            {
                _anim.SetBool("IsWalking", false);
                _agent.isStopped = true;
                _waitTimer += Time.deltaTime;
            }
            else
            {
                _anim.SetBool("IsWalking", true);
                SelectPatrolDirection();
            }
        }

        private void SelectPatrolDirection(bool turn = false)
        {
            _reselectTimer = Random.Range(-1f, 1f);
            _waitTimer = Random.Range(-1f, 0.5f);

            var newDirection = turn ? -transform.forward : new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            var newPosition = transform.position + newDirection.normalized * _agent.speed * ReselectDelay;
            _agent.SetDestination(newPosition);
            _agent.isStopped = false;
            _agent.speed = PatrollingSpeed;
        }

        private void TurnBack()
        {
            SelectPatrolDirection(true);
        }

        private void ResetStatus(Scene scene, LoadSceneMode mode)
        {
            _target = null;
            _patrolling = true;
            _anim.SetBool("IsWalking", true);
        }
    }
}