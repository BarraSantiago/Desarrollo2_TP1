using System;
using Movements;
using UnityEngine;

namespace Enemy
{
    public class Target : MonoBehaviour
    {
        public static Action OnTargetDeath;

        [Header("Targets configuration")] [SerializeField]
        private Movement movement;

        [SerializeField] private float speed = 5f;
        [SerializeField] private float moveDistance = 5f;
        [SerializeField] private float health = 50f;

        [Header("Acceleration options")] [SerializeField]
        private float acceleration = 1f;

        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private Animator animator;

        private float distanceTraveled = 0;
        private float originalSpeed;
        private bool direction = true;

        private Vector3 originalPosition;
        private bool isMovementNotNull;


        private void Start()
        {
            isMovementNotNull = movement != null;
            originalSpeed = speed;
            originalPosition = transform.position;
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