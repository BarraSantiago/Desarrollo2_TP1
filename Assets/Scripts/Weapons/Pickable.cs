using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// The Pickable class manages the behavior of pickable weapons.
    /// The class includes methods for picking up and dropping weapons.
    /// </summary>
    public class Pickable : MonoBehaviour
    {
        [SerializeField] private Transform gunContainer;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private WeaponContainer weaponContainer;
        [SerializeField] private float pickupRange;
        [SerializeField] private float forwardForce = 5;
        [SerializeField] private float upwardForce = 5;

        private Weapon weapon;
        private Rigidbody itemBody;
        private BoxCollider itemCollider;
        private Vector3 distance;

        private void Start()
        {
            weapon = GetComponent<Weapon>();
            if (GetComponent<Rigidbody>() != null)
            {
                itemBody = GetComponent<Rigidbody>();
            }

            if (GetComponent<BoxCollider>() != null)
            {
                itemCollider = GetComponent<BoxCollider>();
            }

            if (!weapon.Equipped)
            {
                weapon.Equipped = false;
                itemBody.isKinematic = false;
                itemCollider.isTrigger = false;
            }
        }

        /// <summary>
        /// Picks ups object to corresponding place (gunContainer).
        /// </summary>
        public void PickUp()
        {
            distance = playerTransform.position - transform.position;

            if (weapon.Equipped || distance.magnitude > pickupRange) return;

            itemBody.isKinematic = true;
            itemCollider.isTrigger = true;
            weaponContainer.EquipWeapon(weapon.Id);


            Transform objectTransform = transform;
            objectTransform.SetParent(gunContainer);
            objectTransform.localPosition = Vector3.zero;
            objectTransform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        /// <summary>
        /// Drops object infront of its position
        /// </summary>
        public void Drop()
        {
            if (!weapon.Equipped) return;

            weapon.Equipped = false;
            weaponContainer.UnequipWeapon(weapon.Id);

            itemCollider.isTrigger = false;

            transform.SetParent(null);

            itemBody.isKinematic = false;
            itemBody.velocity = playerTransform.GetComponent<Rigidbody>().velocity;
            itemBody.AddForce(playerTransform.transform.forward * forwardForce, ForceMode.Impulse);
            itemBody.AddForce(playerTransform.transform.up * upwardForce, ForceMode.Impulse);
        }
    }
}