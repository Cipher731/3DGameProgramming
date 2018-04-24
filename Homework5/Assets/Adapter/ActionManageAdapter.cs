using Action;
using Controller;
using Interface;
using UnityEngine;

namespace Adapter
{
    public class ActionManageAdapter : IPlatePlayerAdapter
    {
        public void PlayPlate(GameObject plate, Vector3 direction)
        {
            GameController.Instance.ActionManager.RunAction(plate, new MoveTowardsDirectionAction(direction));
        }
    }
}