using Audio;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// The Weapon class is an abstract class that serves as a base for different types of weapons the game.
    /// The class includes methods for shooting, reloading, bullet shot reaction, and checking if the weapon can shoot.
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Shoot();
        public int Bullets { get; set; }
        public int MaxBullets { get; set; }
        public int Id { get; protected set; }
        public bool Equipped { get; set; }
        public bool Inventory { get; set; }
        protected SoundEvent OnReload { get; set; }
        protected SoundEvent OnEmptyMagazine { get; set; }
        protected SoundEvent OnShot { get; set; }

        /// <summary>
        /// Gets the currect game object.
        /// </summary>
        /// <returns></returns>
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        
        /// <summary>
        /// Reload bullets and triggers OnReload event.
        /// </summary>
        public void Reload()
        {
            Bullets = MaxBullets;
            OnReload.Raise();
        }

        /// <summary>
        /// Reaction of shooting.
        /// </summary>
        protected void BulletShot()
        {
            Bullets--;
            OnShot?.Raise();
        }

        /// <summary>
        /// Checks for the conditions if a gun can shoot.
        /// </summary>
        /// <returns></returns>
        protected bool CanShoot()
        {
            if (!Equipped) return false;
            if (Bullets > 0)
            {
                return true;
            }

            OnEmptyMagazine.Raise();
            return false;
        }
    }
}