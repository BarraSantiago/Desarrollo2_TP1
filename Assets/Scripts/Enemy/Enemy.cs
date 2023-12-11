using System;
using System.Collections;
using Movements;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Target target;

        [Header("Enemy configuration")] [SerializeField]
        private Movement idleMovement;

        [SerializeField] private Movement chaseMovement;
        [SerializeField] private Movement staticMovement;
        [SerializeField] private Transform player;
        [SerializeField] private float chaseRange = 10;
        [SerializeField] private float attackRange = 2;
        [SerializeField] private float attackDuration = 0.35f;

        private bool isMovementNull;
        private bool isAttacking;
        private enum EnemyState { Idle, Chase, Attack }
        private EnemyState currentState = EnemyState.Idle;
        private float distanceToPlayer;
        private void Start()
        {
            isMovementNull = idleMovement == null || chaseMovement == null;
            
        }

        private void Update()
        {
            EnemyBehavior();
        }

        private void EnemyBehavior()
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            switch (currentState)
            {
                case EnemyState.Idle:
                    IdleBehavior();
                    break;
                case EnemyState.Chase:
                    ChaseBehavior();
                    break;
                case EnemyState.Attack:
                    // Attack behavior is now handled by the coroutine
                    break;
            }
            
            /*
            if (distanceToPlayer > chaseRange)
            {
                target.movement = idleMovement;
            }
            else
            {
                target.originalPosition = player.position;
                    
                if (distanceToPlayer > attackRange)
                {
                    target.movement = chaseMovement;
                }
                else
                {
                    target.movement = staticMovement;
                    target.StartAttack();

                    StartCoroutine(Attacking());
                }
            }*/
        }
        private void IdleBehavior()
        {
            if (distanceToPlayer <= chaseRange)
            {
                currentState = EnemyState.Chase;
            }
            target.movement = idleMovement;
        }

        private void ChaseBehavior()
        {
            if (distanceToPlayer > chaseRange)
            {
                currentState = EnemyState.Idle;
            }
            else if (distanceToPlayer <= attackRange && !isAttacking)
            {
                currentState = EnemyState.Attack;
                StartCoroutine(Attacking());
            }
            else
            {
                target.movement = chaseMovement;
            }
        }
        /// <summary>
        /// attack duration
        /// </summary>
        private IEnumerator Attacking()
        {
            isAttacking = true;
            target.movement = staticMovement;
            target.StartAttack();
            
            yield return new WaitForSeconds(attackDuration);
            
            isAttacking = false;
            currentState = EnemyState.Idle;
        }
    }
}