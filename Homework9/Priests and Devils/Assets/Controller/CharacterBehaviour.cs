using Interface;
using UnityEngine;

namespace Controller
{
    public class CharacterBehaviour : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var action = Director.GetInstance().CurrentSceneController as ISceneAction;
            if (action != null) action.CharacterMove(gameObject);
        }
    }
}