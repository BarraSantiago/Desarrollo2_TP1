﻿using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class InstanceWeapon : MonoBehaviour, IWeapon
    {
        [Header("Instance")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject gunHitbox;

        [Header("Stats")] 
        [SerializeField] private int bullets = 30;
        [SerializeField] private int maxBullets = 30;
        [SerializeField] private float bulletSpeed = 600.0f;
        private bool isActive;

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        /// <summary>
        /// If weapon is equipped (active), instantiate the bullet prefab forward
        /// </summary>
        public void Shoot()
        {
            if (!isActive) return;

            GameObject bullet = Instantiate(bulletPrefab, gunHitbox.transform.position, gunHitbox.transform.rotation);

            bullet.GetComponent<Rigidbody>()?.AddForce(gunHitbox.transform.forward * bulletSpeed);

            Destroy(bullet, 1);
        }

        public int GetBullets()
        {
            return bullets;
        }

        public bool IsEquipped()
        {
            return isActive;
        }

        public void SetEquipped(bool equipped)
        {
            isActive = equipped;
        }

        public void Reload()
        {
            bullets = maxBullets;
        }
    }
}