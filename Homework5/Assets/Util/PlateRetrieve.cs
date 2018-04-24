using UnityEngine;

namespace Util
{
    public class PlateRetrieve : MonoBehaviour
    {
        private void OnBecameInvisible()
        {
            PlateFactory.Instance.RetrievePlate(transform);
        }
    }
}