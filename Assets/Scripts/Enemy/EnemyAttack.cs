using System;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// EnemyAttack handles the enemy's attack interactions.
    /// </summary>
    public class EnemyAttack : MonoBehaviour
    {
        public static Action<float> OnDealDamageEvent;

        [SerializeField] private float damage = 2.5f;
        private const String PlayerTag = "Player";
        
        /// <summary>
        /// On trigger with Player, the OnDealDamageEvent is invoked with 'damage' as the parameter.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                OnDealDamageEvent?.Invoke(damage);
            }
        }

        /// <summary>
        /// On collision with Player, the OnDealDamageEvent is invoked with 'damage' as the parameter.
        /// </summary>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(PlayerTag))
            {
                OnDealDamageEvent?.Invoke(damage);
            }
        }
    }
}