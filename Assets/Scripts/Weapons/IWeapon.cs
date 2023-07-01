﻿using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        public abstract void Shoot();
        public virtual int Bullets { get; set; }
        public int MaxBullets { get; set; }
        public int Id { get; protected set; }
        public abstract void Reload();
        public bool Equipped { get; set; }
        public bool Inventory { get; set; }
      
    }
}