using System;
using System.Collections;
using Movements;
using Player;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// The Target class represents an enemy target in the game.
    /// It manages the target's movements, health, and attack behaviors, and plays sound effects for damage received.
    /// </summary>
    public class Target : MonoBehaviour
    {
        public static Action OnTargetDeath;

        [Header("Targets configuration")] 
        [SerializeField] public Movement movement;
        [SerializeField] private Transform chaseObjective;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float moveDistance = 5f;
        [SerializeField] private float health = 50f;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip recieveDamageSfx;

        [Header("Acceleration options")] [SerializeField]
        private float acceleration = 1f;

        [SerializeField] private float maxSpeed = 10f;

        private Vector3 originalPosition;
        private float distanceTraveled = 0;
        private float originalSpeed;
        private bool direction = true;

        private bool isMovementNotNull;

        #region Animation
        
        private bool isAttacking;
        private bool isDying;
        private bool chase;
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int Death = Animator.StringToHash("death");
        
        #endregion

        private void Awake()
        {
            originalPosition = transform.position;
            chase = chaseObjective != null;
        }

        private void OnEnable()
        {
            InputManager.OnNukeEvent += Die;
        }

        private void OnDisable()
        {
            InputManager.OnNukeEvent -= Die;
        }

        private void Start()
        {
            isMovementNotNull = movement != null;
            originalSpeed = speed;
        }

        private void Update()
        {
            if (isMovementNotNull)
            {
                movement.Move(transform, chase ? chaseObjective.position : originalPosition, ref direction, speed,
                    moveDistance, ref distanceTraveled, acceleration, originalSpeed, maxSpeed, animator);
                
            }
        }

        /// <summary>
        /// Modify target's health
        /// </summary>
        /// <param name="amount"> amount of damage to take </param>
        public void TakeDamage(float amount)
        {
            health -= amount;
            audioSource?.PlayOneShot(recieveDamageSfx);
            
            if (health <= 0 && !isDying)
            {
                Die();
            }
        }

        /// <summary>
        /// Initiates the target's attack sequence.
        /// </summary>
        public void StartAttack()
        {
            if (!isAttacking) StartCoroutine(EndAttack());
        }

        /// <summary>
        /// Ends the target's attack after a delay.
        /// </summary>
        private IEnumerator EndAttack()
        {
            float delay = 0.5f;

            isAttacking = true;
            animator.SetBool(Attack, true);

            yield return new WaitForSeconds(delay);

            isAttacking = false;
            animator.SetBool(Attack, false);
        }


        /// <summary>
        /// On death, invoke event and destroy object.
        /// </summary>
        private void Die()
        {
            OnTargetDeath?.Invoke();
             
            if (animator != null)
            {
                StartCoroutine(DeathAnimation());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Plays the target's death animation and destroys the target after a delay.
        /// </summary>
        private IEnumerator DeathAnimation()
        {
            const float deathDuration = 1.5f;

            isDying = true;
            animator.SetBool(Death, true);
            
            gameObject.GetComponent<Enemy>().enabled = false;
            
            yield return new WaitForSeconds(deathDuration);

            isDying = false;
            
            Destroy(gameObject);
        }
    }
}