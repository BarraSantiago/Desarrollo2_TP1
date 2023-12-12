using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// The WeaponContainer class manages a collection of weapons in a Unity game.
    /// It maintains a dictionary mapping weapon IDs to Weapon objects, and an array of GameObjects representing the weapons.
    /// </summary>
    public class WeaponContainer : MonoBehaviour
    {
        private Dictionary<int, Weapon> idWeapons;

        [SerializeField] private GameObject[] weapons;

        private void Start()
        {
            CreateIdWeapons();
        }

        /// <summary>
        /// Equips the weapon sent by the parameter.
        /// </summary>
        /// <param name="id"> id reference of the weapon</param>
        public void EquipWeapon(int id)
        {
            Weapon weapon = FindWeaponById(id);

            if (weapon == null) return;

            weapon.Inventory = true;

            bool equipWeapon = true;

            foreach (GameObject weapon2 in weapons)
            {
                if (weapon2.GetComponent<Weapon>().Equipped)
                {
                    equipWeapon = false;
                }
            }

            weapon.GetGameObject().SetActive(equipWeapon);
            weapon.Equipped = equipWeapon;
        }

        /// <summary>
        /// Swaps the current weapon equiped with the next weapon in inventory.
        /// </summary>
        public void SwapWeapon()
        {
            Weapon weapon = GetWeapon();

            int id = 0;

            if (weapon != null)
            {
                id = weapon.Id + 1;
                weapon.Equipped = false;
                weapon.GetGameObject().SetActive(false);
            }

            if (id >= weapons.Length) id = 0;

            Weapon weapon2 = FindWeaponById(id);

            if (!FindWeaponById(id).Inventory)
            {
                foreach (GameObject weaponAux in weapons)
                {
                    if (weaponAux.GetComponent<Weapon>().Inventory)
                    {
                        weapon2 = weaponAux.GetComponent<Weapon>();
                    }
                }
            }

            if (!weapon2.Inventory) return;
            
            weapon2.Equipped = true;
            weapon2.GetGameObject().SetActive(true);
        }

        /// <summary>
        /// Unequips the referenced weapon.
        /// </summary>
        /// <param name="id"> weapon id</param>
        public void UnequipWeapon(int id)
        {
            FindWeaponById(id).Inventory = false;
            FindWeaponById(id).Equipped = false;
        }

        /// <summary>
        /// Equiped weapon getter.
        /// </summary>
        public Weapon GetWeapon()
        {
            foreach (GameObject weapon in weapons)
            {
                if (weapon.TryGetComponent(out Weapon weaponComponent) && weaponComponent.Equipped)
                {
                    return weaponComponent;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the weapon referenced by the id
        /// </summary>
        /// <param name="id"> weapon's id </param>
        /// <returns> returns the corresponding weapon, if any. </returns>
        private Weapon FindWeaponById(int id)
        {
            return idWeapons[id];
        }
        
        /// <summary>
        /// Creates the dictionary of weapons and their id
        /// </summary>
        private void CreateIdWeapons()
        {
            idWeapons = new Dictionary<int, Weapon>(weapons.Length);

            foreach (GameObject weapon in weapons)
            {
                idWeapons.Add(weapon.GetComponent<Weapon>().Id, weapon.GetComponent<Weapon>());
            }
        }
    }
}