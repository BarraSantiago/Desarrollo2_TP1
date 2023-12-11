using Audio;
using Enemy;
using UnityEngine;

namespace Weapons
{
    public class BulletHit : MonoBehaviour
    {
        [SerializeField] private float damage = 10;

        [Header("Events")] 
        [SerializeField] private SoundEvent onBulletHit;
        
        /// <summary>
        /// Destroys bullet on collision
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out Target target))
            {
                target.TakeDamage(damage);
                onBulletHit.Raise();
            }

            gameObject.SetActive(false);
        }
    }
}