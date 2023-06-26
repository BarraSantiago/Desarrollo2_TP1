﻿using Audio;
using UnityEngine;

namespace Weapons
{
    public class InstanceWeapon : MonoBehaviour, IWeapon
    {
        [Header("Instance")] [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject gunHitbox;

        [Header("Stats")] [SerializeField] private int bullets = 30;
        [SerializeField] private int maxBullets = 30;
        [SerializeField] private float bulletSpeed = 600.0f;
        [SerializeField] private int id = 0;
        [SerializeField] private float bulletDuration = 1;

        [Header("Events")] 
        [SerializeField] private SoundEvent onInstanceShot;
        private void Awake()
        {
            Id = id;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        /// <summary>
        /// If weapon is equipped (active), instantiate the bullet prefab forward
        /// </summary>
        public void Shoot()
        {
            if (!Equipped) return;

            onInstanceShot.Raise();
            
            SpawnBullet(out GameObject bullet);
            DestroyBullet(bullet);

        }

        private void SpawnBullet(out GameObject bullet)
        {
            bullet = Instantiate(bulletPrefab, gunHitbox.transform.position, transform.rotation);
            bullet.transform.Rotate(0,0,-90);
            bullet.GetComponent<Rigidbody>()?.AddForce(gunHitbox.transform.forward * bulletSpeed);
        }

        private void DestroyBullet(GameObject bullet)
        {
            Destroy(bullet, bulletDuration);
        }

      
        
        public int Bullets { get; set; }
        public int Id { get; set; }

        public bool Equipped { get; set; }
        public bool Inventory { get; set; }

        public void Reload()
        {
            bullets = maxBullets;
        }
    }
}