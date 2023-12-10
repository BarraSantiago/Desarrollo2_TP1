using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Weapons;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        #region Events

        public static Action OnShootEvent;
        public static Action OnReloadEvent;
        public static Action OnPickUpEvent;
        public static Action OnDropEvent;
        public static Action OnSwapWeaponEvent;
        public static Action OnNukeEvent;
        public static Action OnFlashEvent;
        public static Action OnGodModeEvent;
        
        #endregion

        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerController playerController;
        
        private Pickable[] pickables;

        private void Awake()
        {
            Cursor.visible = false;

            pickables = FindObjectsOfType<Pickable>();
        }

        /// <summary>
        /// Player movement logic.
        /// </summary>
        public void OnMove(InputValue context)
        {
            Vector2 movementInput = context.Get<Vector2>();
            playerController.ChangeDirection(movementInput);
        }

        /// <summary>
        /// Increases movement speed
        /// </summary>
        /// <param name="value"> new speed </param>
        public void OnSprint(InputValue value)
        {
            playerController.IsSprinting = value.isPressed;
        }

        /// <summary>
        /// In case of having a weapon equipped, shoots.
        /// </summary>
        public void OnShoot()
        {
            FindObjectOfType<WeaponContainer>().GetWeapon()?.Shoot();
            OnShootEvent?.Invoke();
        }

        /// <summary>
        /// Picks up nearest weapon if in range.
        /// </summary>
        public void OnPickUp()
        {
            foreach (Pickable pickable in pickables)
            {
                pickable.PickUp();
                OnPickUpEvent?.Invoke();
            }
        }

        /// <summary>
        /// Drops the currently equipped weapon, if any.
        /// </summary>
        public void OnDrop()
        {
            foreach (Pickable pickable in pickables)
            {
                pickable.Drop();
                OnDropEvent?.Invoke();
            }
        }

        /// <summary>
        /// Swaps equipped weapon with the next available weapon.
        /// </summary>
        public void OnSwapWeapon()
        {
            OnSwapWeaponEvent?.Invoke();
            FindObjectOfType<WeaponContainer>().SwapWeapon();
        }

        /// <summary>
        /// Resets the bullets count.
        /// </summary>
        public void OnReload()
        {
            FindObjectOfType<WeaponContainer>().GetWeapon()?.Reload();
            OnReloadEvent?.Invoke();
        }

        /// <summary>
        /// Rotates the camera with the mouse or controller's input
        /// </summary>
        public void OnCameraRotation(InputValue context)
        {
            Vector2 cameraMovement = context.Get<Vector2>();
            cameraController.MoveCamera(cameraMovement);
        }

        /// <summary>
        /// Cheat to change to the next scene, if its the last, go back to the first scene.
        /// </summary>
        public void OnNextLevel()
        {
            const int firstSceneIndex = 0;
            int totalScenes = SceneManager.sceneCountInBuildSettings;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex < totalScenes - 1)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(firstSceneIndex);
            }
        }

        /// <summary>
        /// Cheat to enable or disable invincibility
        /// </summary>
        public void OnGodMode()
        {
            OnGodModeEvent?.Invoke();
        }

        /// <summary>
        /// Cheat to increase speed or go back to original speed
        /// </summary>
        public void OnFlash()
        {
            OnFlashEvent?.Invoke();
        }

        /// <summary>
        /// Cheat to kill all targets
        /// </summary>
        public void OnNuke()
        {
            OnNukeEvent?.Invoke();
        }
    }
}