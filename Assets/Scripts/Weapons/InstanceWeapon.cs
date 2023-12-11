using System.Collections;
using Audio;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class InstanceWeapon : Weapon
    {
        [Header("Instance")] 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject gunHitbox;

        [Header("Stats")]
        [SerializeField] private int maxBullets = 30;
        [SerializeField] private float bulletSpeed = 600.0f;
        [SerializeField] private int id = 0;
        [SerializeField] private float bulletDuration = 1;
        
        [Header("Events")] 
        [SerializeField] private SoundEvent onInstanceShotEvent;
        [SerializeField] private SoundEvent onEmptyMagazineEvent;
        [SerializeField] private SoundEvent onReloadEvent;

        private const int MaxBulletPool = 30;
        private GameObject[] bulletPool;
        private void Awake()
        {
            Id = id;
            MaxBullets = maxBullets;
            Bullets = maxBullets;
            OnReload = onReloadEvent;
            OnEmptyMagazine = onEmptyMagazineEvent;
            OnShot = onInstanceShotEvent;

            bulletPool = new GameObject[MaxBulletPool];

            for (int i = 0; i < bulletPool.Length; i++)
            {
                bulletPool[i] = Instantiate(bulletPrefab);
                bulletPool[i].SetActive(false);
            }
        }

        /// <summary>
        /// If weapon is equipped, instantiate the bullet prefab forward
        /// </summary>
        public override void Shoot()
        {
            if (!CanShoot()) return;
            BulletShot();
            
            SpawnBullet(out GameObject bullet);
            
            StartCoroutine(SetBulletInactive(bullet));
        }

        /// <summary>
        /// Instantiates a bullet infront of the gun hitbox going forward.
        /// </summary>
        /// <param name="bullet"> reference to instantiate bullet </param>
        private void SpawnBullet(out GameObject bullet)
        {
            bool instanciated = false;
            bullet = null;
            
            foreach (GameObject bulletP in bulletPool)
            {
                if (bulletP.activeSelf) continue;
                
                bullet = bulletP;
                instanciated = true;
                break;
            }

            if (!instanciated)
            {
                bullet = Instantiate(bulletPrefab);
            }
            
            bullet.SetActive(true);
            
            bullet.transform.position = gunHitbox.transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.Rotate(0,0,-90);
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bullet.GetComponent<Rigidbody>()?.AddForce(gunHitbox.transform.forward * bulletSpeed);
        }

        /// <summary>
        /// Deactivates bullet after bulletDuration time 
        /// </summary>
        /// <param name="bullet"> bullet to be deactivated </param>
        private IEnumerator SetBulletInactive(GameObject bullet)
        {
            yield return new WaitForSeconds(bulletDuration);
            
            bullet.SetActive(false);
        }

    }
}