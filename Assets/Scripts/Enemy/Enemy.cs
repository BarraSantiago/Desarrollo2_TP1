using System.Collections;
using Movements;
using UnityEngine;
using Weapons;

namespace Enemy
{
    /// <summary>
    /// Enemy manages the enemy's behaviour such as movements and attacks, and plays sound effects for attacks.
    /// The enemy can be in one of three states: Idle, Chase, or Attack.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy parameters")] 
        [SerializeField] private Target target;
        [SerializeField] private Movement idleMovement;
        [SerializeField] private Movement chaseMovement;
        [SerializeField] private Movement staticMovement;
        [SerializeField] private Rigidbody enemyRigidbody;
        [SerializeField] private Transform player;
        
        [Header("Enemy configuration")] 
        [SerializeField] private float chaseRange = 10;
        [SerializeField] private float attackRange = 2;
        [SerializeField] private float attackDuration = 0.35f;
        [SerializeField] private float rotationSpeed = 1.35f;
        [SerializeField] private bool isRanged;
        [SerializeField] private Weapon rangedWeapon;

        [Header("Sounds")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip attackSfx;

        private bool isAttacking;

        /// <summary>
        /// The EnemyState enum represents the possible states of the enemy.
        /// </summary>
        private enum EnemyState
        {
            Idle,
            Chase,
            Attack
        }

        private EnemyState currentState = EnemyState.Idle;
        private float distanceToPlayer;

        private void Update()
        {
            EnemyBehaviour();
        }

        /// <summary>
        /// Updates the enemy's behavior based on its current state.
        /// </summary>
        private void EnemyBehaviour()
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
                    
                    Vector3 lookDirection = player.position - enemyRigidbody.position;
                    lookDirection.y = 0;
                    
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    
                    enemyRigidbody.rotation = Quaternion.Slerp(enemyRigidbody.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    break;
            }
        }

        /// <summary>
        /// Defines the behavior of the enemy when in the Idle state.
        /// </summary>
        private void IdleBehavior()
        {
            if (distanceToPlayer <= chaseRange)
            {
                currentState = EnemyState.Chase;
            }

            target.movement = idleMovement;
        }

        /// <summary>
        /// Defines the behavior of the enemy when in the Chase state.
        /// </summary>
        private void ChaseBehavior()
        {
            if (distanceToPlayer > chaseRange)
            {
                currentState = EnemyState.Idle;
            }
            else if (distanceToPlayer <= attackRange && !isAttacking)
            {
                currentState = EnemyState.Attack;
                StartCoroutine(Attack());
            }
            else
            {
                target.movement = chaseMovement;
            }
        }

        /// <summary>
        /// Defines the behavior of the enemy when in the Attack state.
        /// </summary>
        private IEnumerator Attack()
        {
            isAttacking = true;
            target.movement = staticMovement;
            target.StartAttack();
            
            if(isRanged) rangedWeapon.Shoot();

            audioSource.PlayOneShot(attackSfx);

            yield return new WaitForSeconds(attackDuration);

            isAttacking = false;
            currentState = EnemyState.Chase;
        }
    }
}