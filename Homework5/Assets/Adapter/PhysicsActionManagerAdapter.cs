using Action;
using Controller;
using Interface;
using UnityEngine;

namespace Adapter
{
    public class PhysicsActionManagerAdapter : IPlatePlayerAdapter
    {
        public void PlayPlate(GameObject plate, Vector3 direction)
        {
            GameController.Instance.PhysicsActionManager.RunAction(plate, new AddForcePhysicsAction(direction));
        }
    }
}