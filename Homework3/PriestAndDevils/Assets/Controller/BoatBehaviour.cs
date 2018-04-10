using Interface;
using UnityEngine;

namespace Controller
{
    public class BoatBehaviour : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var action = Director.GetInstance().CurrentSceneController as IUserAction;
            if (action != null) action.BoatGo();
        }
    }
}