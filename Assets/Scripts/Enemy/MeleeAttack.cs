using System;
using UnityEngine;

namespace Enemy
{
    public class MeleeAttack : MonoBehaviour
    {
        public static Action<float> OnDealDamageEvent;

        [SerializeField] private float damage = 2.5f;
        private const String PlayerTag = "Player";
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                OnDealDamageEvent?.Invoke(damage);
            }
        }
    }
}