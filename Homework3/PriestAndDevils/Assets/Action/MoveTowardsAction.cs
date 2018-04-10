using UnityEngine;
using Util;

namespace Action
{
    public class MoveTowardsAction : GameAction
    {
        private readonly Vector3 _destination;
        private readonly float _speed;

        public MoveTowardsAction(Vector3 destination, float speed)
        {
            _destination = destination;
            _speed = speed;
        }

        public override void Start()
        {
        }

        public override void Update()
        {
            TargetTransform.position =
                Vector3.MoveTowards(TargetTransform.position, _destination, _speed * Time.deltaTime);
            
            // ReSharper disable once InvertIf
            if (TargetTransform.position == _destination)
            {
                State = ActionState.Completed;
            }
        }
    }
}