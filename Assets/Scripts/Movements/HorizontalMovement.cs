using UnityEngine;

namespace Movements
{
    [CreateAssetMenu(fileName = "movement", menuName = "movements/Horizontal")]
    public class HorizontalMovement : Movement
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
            float newPositionX = objPosition.x + (direction ? speed : -speed) * Time.deltaTime;

            if (Mathf.Abs(newPositionX - originalPos.x) > distance)
            {
                direction = !direction;
            }

            transform.position = new Vector3(newPositionX, transform.position.y, objPosition.z);
        }
    }
}
