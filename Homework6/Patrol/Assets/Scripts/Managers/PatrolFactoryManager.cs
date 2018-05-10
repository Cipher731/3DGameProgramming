using System.Collections.Generic;
using Patrol;
using UnityEngine;
using UnityEngine.AI;

namespace Managers
{
    public class PatrolFactoryManager : MonoBehaviour
    {
        public GameObject PatrolPrefab;

        private readonly Vector3 _birthPlace = new Vector3(20, 0, 13);
        private readonly Vector3 _spawnCenter = new Vector3(-5, 0, -5);

        private readonly List<GameObject> _generatedPatrols = new List<GameObject>(10);

        private Vector3 RandomPoint(Vector3 center, float range)
        {
            for (var i = 0; i < 30; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas) &&
                    Vector3.Distance(_birthPlace, randomPoint) >= 7.5f)
                {
                    return randomPoint;
                }
            }

            return Vector3.zero;
        }

        public void ArrangePatrol(int num, float speed)
        {
            for (var i = num; i < _generatedPatrols.Count; i++)
            {
                _generatedPatrols[i].SetActive(false);
            }
            
            while (_generatedPatrols.Count < num)
            {
                var patrol = Instantiate(PatrolPrefab);
                DontDestroyOnLoad(patrol);
                _generatedPatrols.Add(patrol);
            }

            for (var i = 0; i < num; i++)
            {
                var newPatrol = _generatedPatrols[i].transform;
                newPatrol.position = RandomPoint(_spawnCenter, 20f);
                var patrolAi = newPatrol.GetComponent<PatrolAi>();
                patrolAi.ChasingSpeed = speed;
                patrolAi.InitializeState();
            }
        }
    }
}