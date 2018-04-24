using UnityEngine;
using Util;

namespace Action
{
    public class MoveTowardsDirectionAction : GameAction
    {
        private readonly Vector3 _speed;

        public MoveTowardsDirectionAction(Vector3 speed)
        {
            _speed = speed;
        }

        public override void Start()
        {
        }

        public override void Update()
        {
            TargetTransform.Translate(_speed * Time.deltaTime);
        }
    }
}