using UnityEngine;
using Interface;

namespace Util
{
    public abstract class PhysicsGameAction
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

        public IPhysicsActionCallback Callback { get; set; }

        public abstract void Start();
        public abstract void FixedUpdate();
    }
}