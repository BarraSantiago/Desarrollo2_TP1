using UnityEngine;

namespace Player
{
    /// <summary>
    /// The CameraController class manages the camera movement in the game.
    /// It rotates the camera based on mouse input, allowing the player to look around in the game world.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;

        [SerializeField] private Transform orientation;
        
        private float xRotation;
        private float yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /// <summary>
        /// Rotates the camera to follow the mouse.
        /// </summary>
        public void MoveCamera(Vector2 cameraRotation)
        {
            Vector2 cameraMovement = cameraRotation;
            float xMovement = cameraMovement.x  * Time.deltaTime * sensitivityX;
            float yMovement = cameraMovement.y * Time.deltaTime * sensitivityY;

            yRotation += xMovement;
            xRotation -= yMovement;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}