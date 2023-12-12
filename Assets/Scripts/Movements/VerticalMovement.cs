using UnityEngine;

namespace Movements
{
    /// <summary>
    /// Vertical movement represents the vertical move variation of the movement class
    /// </summary>
    [CreateAssetMenu(fileName = "movement", menuName = "movements/Vertical")]
    public class VerticalMovement : Movement
    {
        /// <summary>
        /// Moves the object (target) depending on its corresponding behaviour.
        /// </summary>
        /// <param name="transform"> Object's body </param>
        /// <param name="originalPos"> Reference to object's original positon </param>
        /// <param name="direction"> Movement's direction </param>
        /// <param name="speed"> Movement's speed </param>
        /// <param name="distance"> Distance it has to move </param>
        /// <param name="distanceTraveled"> Distance moved </param>
        /// <param name="acceleration"> Acceletarion over time </param>
        /// <param name="ogSpeed"> Original speed </param>
        /// <param name="maxSpeed"> Max speed it can reach </param>
        public override void Move(Transform transform, Vector3 originalPos, ref bool direction, float speed,
            float distance, ref float distanceTraveled, float acceleration, float ogSpeed, float maxSpeed, Animator animator)
        {
            Vector3 objPosition = transform.position;

            float newPositionY = objPosition.y + (direction ? speed : -speed) * Time.deltaTime;

            if (Mathf.Abs(newPositionY - originalPos.y) > distance)
            {
                direction = !direction;
            }

            transform.position = new Vector3(transform.position.x, newPositionY, objPosition.z);
        }
    }
}