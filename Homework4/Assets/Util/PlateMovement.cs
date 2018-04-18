using UnityEngine;

namespace Util
{
    public class PlateMovement : MonoBehaviour
    {
        public Vector3 Speed;

        private void Update()
        {
            transform.Translate(Speed * Time.deltaTime);
        }

        private void OnBecameInvisible()
        {
            PlateFactory.Instance.RetrievePlate(transform);
        }
    }
}