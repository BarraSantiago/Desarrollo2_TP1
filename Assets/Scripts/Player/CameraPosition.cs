using UnityEngine;

namespace Player
{
    /// <summary>
    /// The CameraPosition class is a MonoBehaviour that synchronizes the position of a specified camera with the GameObject it's attached to.
    /// </summary>
    public class CameraPosition : MonoBehaviour
    {
        [SerializeField] private Transform cameraPosition;

        private void LateUpdate()
        {
            cameraPosition.position = transform.position;
        }
    }
}
