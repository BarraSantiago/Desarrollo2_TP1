using System;
using System.Collections;
using Audio;
using Enemy;
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
        public static Action<float> OnRecieveDamageEvent;

        [Header("Player settings")] 
        [SerializeField] private float sprintingSpeed = 13.0f;

        [SerializeField] private float walkingSpeed = 8.0f;
        [SerializeField] private float flashSpeedMultiplier = 3.0f;
        [SerializeField] private float invulnerabilityTimer = 0.2f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private SoundEvent onDamaged;

        [SerializeField] private bool startTimer = true;
        [SerializeField] private int levelTimer = 30;

        private const float GravityValue = -9.81f;

        private Vector3 desiredDirection;
        private Vector3 playerVelocity;

        private bool isFlashMode;
        private bool isGodMode;
        private bool isInvulnerable;

        private void OnEnable()
        {
            InputManager.OnFlashEvent += FlashMode;
            InputManager.OnGodModeEvent += GodMode;
            EnemyAttack.OnDealDamageEvent += RecieveDamage;
        }

        private void OnDisable()
        {
            InputManager.OnFlashEvent -= FlashMode;
            InputManager.OnGodModeEvent -= GodMode;
            EnemyAttack.OnDealDamageEvent -= RecieveDamage;
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
            while (Timer > 0)
            {
                Timer -= isGodMode ? 0 : Time.deltaTime;
                yield return null;
            }

            if (Timer > 0) yield break;

            OnDefeatEvent?.Invoke();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Reduces the player timer
        /// </summary>
        /// <param name="damage"> Amount of time decreased from player's timer </param>
        private void RecieveDamage(float damage)
        {
            if (!isInvulnerable && !isGodMode)
            {
                Timer -= damage;
                onDamaged.Raise();
                OnRecieveDamageEvent?.Invoke(damage);
                
                StartCoroutine(Invulnerability());
            }
        }

        /// <summary>
        /// Makes the player invulnerable for a time
        /// </summary>
        /// <returns></returns>
        private IEnumerator Invulnerability()
        {
            isInvulnerable = true;

            yield return new WaitForSeconds(invulnerabilityTimer);

            isInvulnerable = false;
        }
    }
}