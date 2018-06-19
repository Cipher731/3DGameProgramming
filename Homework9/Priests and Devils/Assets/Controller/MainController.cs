using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Controller
{
    public class MainController : MonoBehaviour, ISceneController, ISceneAction
    {
        private ActionManager _actionManager;

        private readonly Hashtable _characterStat = new Hashtable();
        private float _bankSlotLocalHeight;

        private GameObject _boat;
        private bool _boatAtLeft;

        private GameGui _gameGui;

        private bool _isBusy;
        private GameObject _leftBank;
        private Vector3 _leftDock;
        private GameObject _rightBank;
        private Vector3 _rightDock;

        private StatusGraph _graph;

        public void LoadResources()
        {
            _actionManager = gameObject.AddComponent<ActionManager>();

            var halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
            var halfHeight = Camera.main.orthographicSize;

            // Initialize GUI
            _gameGui = gameObject.AddComponent<GameGui>();

            // Place bank
            var bank = Resources.Load<GameObject>("Prefab/Bank");
            var leftBankPosition = new Vector3(-halfWidth + bank.transform.lossyScale.x / 2,
                -halfHeight + bank.transform.lossyScale.y / 2);
            _leftBank = Instantiate(bank, leftBankPosition, Quaternion.identity);
            _rightBank = Instantiate(bank, Vector3.Reflect(leftBankPosition, Vector3.right), Quaternion.identity);

            // Place water
            var waterCube = Resources.Load<GameObject>("Prefab/WaterCube");
            var water = Instantiate(waterCube, new Vector3(0, -halfHeight + 1), Quaternion.identity);
            water.transform.localScale = new Vector3(2 * (halfWidth - bank.transform.lossyScale.x), 2, 1);

            // Calculate boat position
            var boat = Resources.Load<GameObject>("Prefab/Boat");
            _leftDock = new Vector3(-halfWidth + bank.transform.lossyScale.x + boat.transform.lossyScale.x / 2, -2.5f);
            _rightDock = Vector3.Reflect(_leftDock, Vector3.right);

            // Place boat
            _boat = Instantiate(boat, _rightDock, Quaternion.identity);
            // Not useful anymore in action drived model.
            // _boat.AddComponent<Movement>();
            _boat.AddComponent<BoatBehaviour>();

            // Place characters
            var priest = Resources.Load<GameObject>("Prefab/Priest");
            var devil = Resources.Load<GameObject>("Prefab/Devil");

            for (var i = 0; i < 6; i++)
            {
                var character = Instantiate(i < 3 ? priest : devil, Vector3.zero, Quaternion.identity);
                character.AddComponent<CharacterBehaviour>();
                // Not useful anymore in action drived model.
                // character.AddComponent<Movement>();

                character.transform.parent = _rightBank.transform;
                _bankSlotLocalHeight = 0.5f + character.transform.localScale.y / 2;

                character.transform.localPosition = new Vector3(GetLocalBankSlotX(i), _bankSlotLocalHeight);

                _characterStat[character] = new CharacterStatus(i < 3 ? CharacterType.Priest : CharacterType.Devil, i);
            }

            _graph = new StatusGraph(3, 3);
        }

        public void BoatGo()
        {
            if (_isBusy) return;

            _isBusy = true;
            StartCoroutine(BoatGoCoroutine());
        }

        public void CharacterMove(GameObject character)
        {
            if (_isBusy) return;

            _isBusy = true;
            StartCoroutine(CharacterMoveCoroutine(character));
        }

        public void StartGame()
        {
            SceneManager.LoadScene(0);
            LoadResources();
        }

        private void Awake()
        {
            var director = Director.GetInstance();
            director.CurrentSceneController = this;
            Director.SetFps(60);
            LoadResources();
        }

        private float GetLocalBankSlotX(int index, bool isLeft = false)
        {
            var interval = 1 / _leftBank.transform.lossyScale.x;
            return isLeft ? 0.5f - (index + 1) * interval : -(0.5f - (index + 1) * interval);
        }

        private IEnumerator BoatGoCoroutine()
        {
            if (_boat.transform.childCount == 0)
            {
                _isBusy = false;
                yield break;
            }

            _boat.MoveTowards(_boatAtLeft ? _rightDock : _leftDock);
            _boatAtLeft = !_boatAtLeft;
            yield return new WaitUntil(_boat.IsIdle);
            _isBusy = false;
            CheckGameCondition();
        }

        private IEnumerator CharacterMoveCoroutine(GameObject character)
        {
            var status = _characterStat[character] as CharacterStatus;
            if (status == null)
            {
                _isBusy = false;
                yield break;
            }

            Vector3 localDestination;

            if (status.OnBoard)
            {
                character.transform.parent = (_boatAtLeft ? _leftBank : _rightBank).transform;
                localDestination = new Vector3(GetLocalBankSlotX(status.BankSlot, _boatAtLeft),
                    0.5f + character.transform.localScale.y / 2);
            }
            else
            {
                if (_boat.transform.childCount == 2 || !IsSameSideWithBoat(character))
                {
                    _isBusy = false;
                    yield break;
                }

                character.transform.parent = _boat.transform;
                localDestination = new Vector3(GetLocalBoatSlotX(), 0.5f + character.transform.localScale.y / 2);
            }

            status.OnBoard = !status.OnBoard;
            var globalDestination = character.transform.parent.TransformPoint(localDestination);
            character.JumpTo(globalDestination);

            yield return new WaitUntil(character.IsIdle);
            _isBusy = false;
        }

        private bool IsSameSideWithBoat(GameObject character)
        {
            return _boatAtLeft && character.transform.parent == _leftBank.transform ||
                   !_boatAtLeft && character.transform.parent == _rightBank.transform;
        }

        private float GetLocalBoatSlotX()
        {
            const float leftSlot = -0.2f;
            const float rightSlot = 0.2f;
            if (_boat.transform.childCount == 2)
                return _boat.transform.GetChild(0).transform.localPosition.x > 0 ? leftSlot : rightSlot;

            return leftSlot;
        }

        private void CheckGameCondition()
        {
            var left = _leftBank.transform.GetComponentsInChildren<Transform>();
            var right = _rightBank.transform.GetComponentsInChildren<Transform>();
            var boat = _boat.transform.GetComponentsInChildren<Transform>();

            if (_boatAtLeft && left.Length + boat.Length == 8)
            {
                _gameGui.Win();
                _isBusy = true;
            }
            else if (_boatAtLeft
                ? CheckDanger(left, boat) || CheckDanger(right, new Transform[] { })
                : CheckDanger(right, boat) || CheckDanger(left, new Transform[] { }))
            {
                _gameGui.Fail();
                _isBusy = true;
            }
        }

        private int CountCharacter(IEnumerable<Transform> array, CharacterType type)
        {
            return array.Count(e => CheckType(e.gameObject, type));
        }

        private bool CheckType(GameObject obj, CharacterType type)
        {
            var characterStatus = _characterStat[obj] as CharacterStatus;
            return characterStatus != null && characterStatus.Type == type;
        }

        private bool CheckDanger(Transform[] array1, Transform[] array2)
        {
            var devils = CountCharacter(array1, CharacterType.Devil) + CountCharacter(array2, CharacterType.Devil);
            var priests = CountCharacter(array1, CharacterType.Priest) + CountCharacter(array2, CharacterType.Priest);
            return devils > priests && priests > 0;
        }

        public ActionManager GetActionManager()
        {
            return _actionManager;
        }

        public void Help()
        {
            if (_isBusy)
            {
                return;
            }
            _isBusy = true;
            
            var left = _leftBank.transform.GetComponentsInChildren<Transform>();
            var right = _rightBank.transform.GetComponentsInChildren<Transform>();
            var node = _graph.FindNodeByStatus(new[]
                {
                    CountCharacter(left, CharacterType.Priest),
                    CountCharacter(left, CharacterType.Devil),
                    CountCharacter(right, CharacterType.Priest),
                    CountCharacter(right, CharacterType.Devil),
                    _boatAtLeft ? 1 : 0
                }
            );
            if (node == null)
            {
                _isBusy = false;
                return;
            }
            var transport = node.GetNextTransport();
            StartCoroutine(AutoCharacterMoveCoroutine(transport));
        }

        private IEnumerator AutoCharacterMoveCoroutine(StatusGraph.TransportAction transport)
        {
            var priest = transport.Priest;
            var devil = transport.Devil;
            var currentBank = _boatAtLeft ? _leftBank : _rightBank;
            var currentBankTransform = currentBank.transform;
            
            while (priest > 0)
            {
                var character = currentBankTransform.GetComponentsInChildren<Transform>()
                    .First(e => CheckType(e.gameObject, CharacterType.Priest));
                yield return StartCoroutine(CharacterMoveCoroutine(character.gameObject));
                priest--;
            }

            while (devil > 0)
            {
                var character = currentBankTransform.GetComponentsInChildren<Transform>()
                    .First(e => CheckType(e.gameObject, CharacterType.Devil));
                yield return StartCoroutine(CharacterMoveCoroutine(character.gameObject));
                devil--;
            }

            yield return StartCoroutine(BoatGoCoroutine());
            
            CheckGameCondition();

            if (_isBusy)
            {
                yield break;
            }
            
            foreach (var character in _boat.transform.GetComponentsInChildren<Transform>())
            {
                yield return StartCoroutine(CharacterMoveCoroutine(character.gameObject));
            }

            _isBusy = false;
        }
    }
}