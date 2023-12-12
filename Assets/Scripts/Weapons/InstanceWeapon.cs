using System.Collections;
using Audio;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// The InstanceWeapon class manages the behavior of instance weapons.
    /// The class includes methods for shooting, spawning bullets, and setting bullets inactive after a certain duration.
    /// It also maintains a bullet pool for efficient bullet instantiation and a spray pattern for bullet deviation.
    /// </summary>
    public class InstanceWeapon : Weapon
    {
        [Header("Instance")] 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject gunHitbox;

        [Header("Stats")] 
        [SerializeField] private bool isEquipped = false;
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

        #region BulletSprayPattern

        private int currentBulletIndex = 0;

        /// <summary>
        /// Deviation spray pattern for bullets
        /// </summary>
        private readonly Vector3[] sprayPattern =
        {
            new Vector3(.05f, 0.05f, 0f),
            new Vector3(.01f, -.05f, 0f),
            new Vector3(.01f, 0.08f, 0f),
            new Vector3(-.03f, 0.03f, 0f),
            new Vector3(-.05f, 0.05f, 0f),
            new Vector3(-.05f, -0.05f, 0f),
            new Vector3(-.05f, 0f, 0f),
            new Vector3(.08f, 0f, 0f),
            new Vector3(0f, -0.05f, 0f),
            new Vector3(-0.05f, 0.05f, 0f)
        };

        #endregion

        private void Awake()
        {
            Id = id;
            MaxBullets = maxBullets;
            Bullets = maxBullets;
            OnReload = onReloadEvent;
            OnEmptyMagazine = onEmptyMagazineEvent;
            OnShot = onInstanceShotEvent;
            Equipped = isEquipped;
            
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
            Vector3 sprayDeviation = sprayPattern[currentBulletIndex];
            currentBulletIndex = (currentBulletIndex + 1) % sprayPattern.Length;
            
            SpawnBullet(out GameObject bullet, sprayDeviation);

            StartCoroutine(SetBulletInactive(bullet));
        }

        /// <summary>
        /// Instantiates a bullet infront of the gun hitbox going forward.
        /// </summary>
        /// <param name="bullet"> reference to instantiate bullet </param>
        private void SpawnBullet(out GameObject bullet, Vector3 deviation)
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
            bullet.transform.Rotate(0, 0, -90);
            
            Rigidbody bulletRigidb = bullet.GetComponent<Rigidbody>();
            bulletRigidb.velocity = Vector3.zero;
            bulletRigidb.angularVelocity = Vector3.zero;
            
            Vector3 bulletDevi =  new (gunHitbox.transform.forward.x + deviation.x, gunHitbox.transform.forward.y + 
                deviation.y, gunHitbox.transform.forward.z + deviation.z);
            
            bulletRigidb.AddForce(bulletDevi * bulletSpeed);
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