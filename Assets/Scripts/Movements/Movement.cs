using UnityEngine;

namespace Movements
{
    /// <summary>
    /// Movement interface to make many types movements
    /// </summary>
    public abstract class Movement : ScriptableObject
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
        /// <param name="animator"> Entity animator </param>
        public abstract void Move(Transform transform, Vector3 originalPos, ref bool direction, float speed,
            float distance, ref float distanceTraveled, float acceleration, float ogSpeed, float maxSpeed, Animator animator);
    }
}