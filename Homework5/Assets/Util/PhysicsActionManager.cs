using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;

namespace Util
{
    public class PhysicsActionManager : MonoBehaviour, IPhysicsActionCallback
    {
        private readonly List<PhysicsGameAction> _actions = new List<PhysicsGameAction>();
        private readonly Queue<PhysicsGameAction> _toAdd = new Queue<PhysicsGameAction>();
        private readonly Queue<PhysicsGameAction> _toDetele = new Queue<PhysicsGameAction>();

        public void RunAction(GameObject obj, PhysicsGameAction action)
        {
            action.TargetGameObject = obj;
            action.Callback = this;
            
            _toAdd.Enqueue(action);
            action.Start();
        }

        private void FixedUpdate()
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
                        item.FixedUpdate();
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

        public void ActionEvent(PhysicsGameAction source, Type paramType = null, object param = null)
        {
        }
        
        public bool GameObjectIsBusy(GameObject obj)
        {
            return _actions.Union(_toAdd).Any(act => act.TargetGameObject == obj);
        }

        public void EraseAction(GameObject obj)
        {
            _actions.ForEach(action =>
            {
                if (action.TargetGameObject == obj)
                {
                    _toDetele.Enqueue(action);
                }
            });
        }
    }
}