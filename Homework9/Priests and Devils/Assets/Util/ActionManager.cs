using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;

namespace Util
{
    public class ActionManager : MonoBehaviour, IActionCallback
    {
        private readonly List<GameAction> _actions = new List<GameAction>();
        private readonly Queue<GameAction> _toAdd = new Queue<GameAction>();
        private readonly Queue<GameAction> _toDetele = new Queue<GameAction>();

        public void RunAction(GameObject obj, GameAction action)
        {
            action.TargetGameObject = obj;
            action.Callback = this;

            _toAdd.Enqueue(action);
            action.Start();
        }

        private void Update()
        {
            while (_toAdd.Count > 0)
            {
                _actions.Add(_toAdd.Dequeue());
            }

            foreach (var item in _actions)
            {
                switch (item.State)
                {
                    case ActionState.Started:
                        item.Update();
                        break;
                    case ActionState.Completed:
                        _toDetele.Enqueue(item);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            while (_toDetele.Count > 0)
            {
                _actions.Remove(_toDetele.Dequeue());
            }
        }

        public void ActionEvent(GameAction source, Type paramType = null, object param = null)
        {
        }

        public bool GameObjectIsBusy(GameObject obj)
        {
            return _actions.Union(_toAdd).Any(act => act.TargetGameObject == obj);
        }
    }
}