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
        private float timer = 0;

        private void Start()
        {
            isMovementNull = idleMovement == null || chaseMovement == null;

            StartCoroutine(EnemyBehavior());
        }

        private IEnumerator EnemyBehavior()
        {
            while (!isMovementNull)
            {
                if (Vector3.Distance(transform.position, player.position) > chaseRange)
                {
                    target.movement = idleMovement;
                }
                else
                {
                    target.originalPosition = player.position;
                    if (Vector3.Distance(transform.position, player.position) > attackRange)
                    {
                        target.movement = chaseMovement;
                    }
                    else
                    {
                        target.movement = staticMovement;
                        target.StartAttack();
                        yield return new WaitForSeconds(attackDuration);
                    }
                }

                yield return null;
            }
        }
    }
}