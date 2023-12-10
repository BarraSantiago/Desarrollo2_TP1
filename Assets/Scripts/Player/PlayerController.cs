using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Class that enables a character to be moved
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public bool IsSprinting { get; set; }
        public float Timer { get; set; }
        
        public static Action OnDefeatEvent;
        
        [Header("Player settings")]
        [SerializeField] private float sprintingSpeed = 13.0f;
        [SerializeField] private float walkingSpeed = 8.0f;
        [SerializeField] private float flashSpeedMultiplier = 3.0f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CharacterController characterController;

        [SerializeField] private bool startTimer = true;
        [SerializeField] private int levelTimer = 30;

        private const float GravityValue = -9.81f;

        private Vector3 desiredDirection;
        private Vector3 playerVelocity;

        private bool isFlashMode;
        private bool isGodMode;

        private void OnEnable()
        {
            InputManager.OnFlashEvent += FlashMode;
            InputManager.OnGodModeEvent += GodMode;
        }

        private void OnDisable()
        {
            InputManager.OnFlashEvent -= FlashMode;
            InputManager.OnGodModeEvent -= GodMode;
        }

        private void Start()
        {
            if (Camera.main != null) cameraTransform = Camera.main.transform;
            
            Timer = levelTimer;

            if (startTimer)
            {
                StartCoroutine(TimerCoroutine());
            }
        }

        private void Update()
        {
            Vector3 currentDirection = GetCharacterDirection();
            characterController.Move(
                currentDirection * (Time.deltaTime * (IsSprinting ? sprintingSpeed : walkingSpeed)));
        }

        /// <summary>
        /// Recieves the direction the user wants to move in and saves it in desiredDirection.
        /// </summary>
        /// <param name="movement"> direction from the Input </param>
        public void ChangeDirection(Vector2 movement)
        {
            desiredDirection = new Vector3(movement.x, 0, movement.y);
        }

        /// <summary>
        /// Modifies the direction in which the player is moving relative to camera direction and gravity.
        /// </summary>
        /// <returns> New transform direction </returns>
        private Vector3 GetCharacterDirection()
        {
            Vector3 transformDirection = cameraTransform.TransformDirection(desiredDirection);

            transformDirection.y = characterController.isGrounded ? 0 : GravityValue;

            return transformDirection;
        }

        /// <summary>
        /// Enables Flash mode increasing player speed or disables it
        /// </summary>
        private void FlashMode()
        {
            if (!isFlashMode)
            {
                walkingSpeed *= flashSpeedMultiplier;
                sprintingSpeed *= flashSpeedMultiplier;
            }
            else
            {
                walkingSpeed /= flashSpeedMultiplier;
                sprintingSpeed /= flashSpeedMultiplier;
            }

            isFlashMode = !isFlashMode;
        }

        /// <summary>
        /// Enables or disables God mode that turns player invulnerable
        /// </summary>
        private void GodMode()
        {
            isGodMode = !isGodMode;
        }
        
        /// <summary>
        /// Calculates time remaining until the player loses
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimerCoroutine()
        {
            if(isGodMode) yield break;
            
            while (Timer > 0)
            {
                Timer -= Time.deltaTime;
                yield return null;
            }
            
            if ((Timer > 0)) yield break;
            
            OnDefeatEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}