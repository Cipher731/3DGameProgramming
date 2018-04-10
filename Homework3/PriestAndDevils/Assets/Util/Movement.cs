using Action;
using UnityEngine;

namespace Util
{
    public static class MovementExtension
    {
        private static ActionManager Manager
        {
            get { return Director.GetInstance().CurrentSceneController.GetActionManager(); }
        }
        
        public static void MoveTowards(this GameObject gameObject, Vector3 destination)
        {
            Manager.RunAction(gameObject, new MoveTowardsAction(destination, 10));
        }

        public static void JumpTo(this GameObject gameObject, Vector3 destination)
        {
            var position = gameObject.transform.position;
            Manager.RunAction(gameObject, new SequnceAction(new []
            {
                new MoveTowardsAction(new Vector3(position.x, 1.5f), 20),
                new MoveTowardsAction(new Vector3(destination.x, 1.5f), 10),
                new MoveTowardsAction(destination, 20)
            }));
        }
        
        public static bool IsIdle(this GameObject gameObject)
        {
            return !Manager.GameObjectIsBusy(gameObject);
        }
    }
}