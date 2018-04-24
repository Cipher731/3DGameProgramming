using UnityEngine;
using Util;

namespace Action
{
    public class AddForcePhysicsAction : PhysicsGameAction
    {
        private readonly Vector3 _force;

        public AddForcePhysicsAction(Vector3 force)
        {
            _force = force;
        }
        
        public override void Start()
        {
        }

        public override void FixedUpdate()
        {
            var rigidbody = TargetGameObject.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.AddForce(_force);
            }
        }
    }
}