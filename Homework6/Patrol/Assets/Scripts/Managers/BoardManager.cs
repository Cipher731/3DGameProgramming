using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class BoardManager : MonoBehaviour
    {
        private readonly Vector3[] _exitPositions =
        {
            new Vector3(-11, 10, 20),
            new Vector3(-20, 10, 6),
            new Vector3(-11, 10, 9),
            new Vector3(-2, 10, 21),
            new Vector3(5.5f, 10, -14.3f),
            new Vector3(14, 10, -5)
        };

        [HideInInspector] public PatrolFactoryManager FactoryManager;
        public GameObject ExitPrefab;

        public int BasePatrolNumber = 4;
        public int PatrolsAdditionPerLevel = 2;

        public float BaseSpeed = 3.5f;
        public float SpeedAdditionPerLevel = 0.1f;

        private GameObject _exit;

        public void SetupScene(int level)
        {
            CreateExit();
            FactoryManager.ArrangePatrol(
                BasePatrolNumber + level * PatrolsAdditionPerLevel,
                BaseSpeed + level * SpeedAdditionPerLevel);
        }

        private void CreateExit()
        {
            if (_exit != null)
            {
                Destroy(_exit);
            }
            var exitPosition = _exitPositions[Random.Range(0, 6)];
            _exit = Instantiate(ExitPrefab, exitPosition, Quaternion.identity);
        }
    }
}