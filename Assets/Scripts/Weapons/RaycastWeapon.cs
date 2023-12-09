using System.Collections;
using Audio;
using Enemy;
using UnityEngine;
using UnityEngine.UIElements;

namespace Weapons
{
    public class RaycastWeapon : Weapon
    {
        [Header("Raycast")] 
        [SerializeField] private float range = 100f;
        [SerializeField] private LineRenderer laserRenderer;
        [SerializeField] private Transform laserOrigin;
        [SerializeField] private float laserDuration = 0.05f;

        [Header("Stats")] 
        [SerializeField] private float damage = 10f;
        [SerializeField] private int maxBullets = 10;
        [SerializeField] private int id = 1;
        
        [Header("Events")] 
        [SerializeField] private SoundEvent onRaycastShootEvent;
        [SerializeField] private SoundEvent onEmptyMagazineEvent;
        [SerializeField] private SoundEvent onReloadEvent;

        private void Awake()
        {
            Id = id;
            MaxBullets = maxBullets;
            Bullets = maxBullets;
            OnReload = onReloadEvent;
            OnShot = onRaycastShootEvent;
            OnEmptyMagazine = onEmptyMagazineEvent;
            laserRenderer = GetComponent<LineRenderer>();
        }

        /// <summary>
        /// Casts a ray going forward
        /// </summary>
        public override void Shoot()
        {
            const int laserStartId = 0;
            const int laserEndId = 1;
            
            if(!CanShoot()) return;
            BulletShot();
            
            RaycastHit hit;
            
            laserRenderer.SetPosition(laserStartId, laserOrigin.position);
            if (Physics.Raycast(Camera.main!.transform.position, Camera.main!.transform.forward, out hit, range))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null) target.TakeDamage(damage);
                laserRenderer.SetPosition(laserEndId, hit.point);
            }
            else
            {
                laserRenderer.SetPosition(laserEndId, laserOrigin.position + Camera.main.transform.forward * range);
                
            }
            StartCoroutine(ShowLaser());
        }

        /// <summary>
        /// Enables and disables the laser line renderer
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowLaser()
        {
            laserRenderer.enabled = true;
            yield return new WaitForSeconds(laserDuration);
            laserRenderer.enabled = false;
        }
        
    }
}