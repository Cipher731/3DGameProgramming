using UnityEngine;
using Interface;

namespace Util
{
    public enum ActionState
    {
        Started,
        Completed
    }
    
    public abstract class GameAction
    {
        private GameObject _targetGameObject;
        protected Transform TargetTransform;
        public ActionState State = ActionState.Started;

        public GameObject TargetGameObject
        {
            get { return _targetGameObject; }
            set
            {
                _targetGameObject = value;
                TargetTransform = _targetGameObject.transform;
            }
        }

        public IActionCallback Callback { get; set; }

        public abstract void Start();
        public abstract void Update();
    }
}