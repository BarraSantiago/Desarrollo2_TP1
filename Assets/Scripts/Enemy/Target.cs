using System;
using System.Collections;
using Movements;
using Player;
using UnityEngine;

namespace Enemy
{
    public class Target : MonoBehaviour
    {
        public static Action OnTargetDeath;
        public Vector3 originalPosition;

        [Header("Targets configuration")] [SerializeField]
        public Movement movement;

        [SerializeField] private float speed = 5f;
        [SerializeField] private float moveDistance = 5f;
        [SerializeField] private float health = 50f;
        [SerializeField] private Animator animator;

        [Header("Acceleration options")] [SerializeField]
        private float acceleration = 1f;

        [SerializeField] private float maxSpeed = 10f;

        private float distanceTraveled = 0;
        private float originalSpeed;
        private bool direction = true;

        private bool isMovementNotNull;
        private bool isAttacking;
        private const string AttackKey = "attack";
        private static readonly int Attack = Animator.StringToHash(AttackKey);

        private void Awake()
        {
            originalPosition = transform.position;
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
                movement.Move(transform, originalPosition, ref direction, speed, moveDistance, ref distanceTraveled,
                    acceleration, originalSpeed, maxSpeed, animator);
        }

        /// <summary>
        /// Modify target's health
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(float amount)
        {
            health -= amount;
            
            if (health <= 0)
            {
                Die();
            }
        }

        public void StartAttack()
        {
            if(!isAttacking) StartCoroutine(EndAttack());
        }

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
            Destroy(gameObject);
        }
    }
}