using System;
using Movements;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public static Action OnTargetDeath;

        [Header("Targets configuration")] 
        [SerializeField] private Movement idleMovement;
        [SerializeField] private Movement chaseMovement;
        [SerializeField] private Transform player;
        [SerializeField] private Animator animator;

        [SerializeField] private float speed = 5f;
        [SerializeField] private float moveDistance = 5f;
        [SerializeField] private float health = 50f;

        [Header("Acceleration options")] [SerializeField]
        private float acceleration = 1f;

        [SerializeField] private float maxSpeed = 10f;

        private float distanceTraveled = 0;
        private float originalSpeed;
        private bool direction = true;

        private Vector3 originalPosition;
        private bool isMovementNull;


        private void Start()
        {
            isMovementNull = idleMovement == null;
            originalSpeed = speed;
            originalPosition = transform.position;
        }

        private void Update()
        {
            if (isMovementNull) return;
            
            if (Vector3.Distance(transform.position, player.position) > 5)
            {
                Move(idleMovement);
            }
            else
            {
                Move(chaseMovement);
            }
        }

        private void Move(Movement movement)
        {
            movement.Move(transform, player.transform.position, ref direction, speed, moveDistance, ref distanceTraveled,
                acceleration, originalSpeed, maxSpeed, animator);
        }
    }
}