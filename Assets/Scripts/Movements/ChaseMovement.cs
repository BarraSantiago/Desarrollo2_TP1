using UnityEngine;

namespace Movements
{
    
    /// <summary>
    /// Chase movement represents the chasing an object variation of the movement class
    /// </summary>
    [CreateAssetMenu(fileName = "movement", menuName = "movements/Chase")]
    public class ChaseMovement : Movement
    {
        [SerializeField] private float rotationSpeed = 2;

        public override void Move(Transform transform, Vector3 originalPos, ref bool direction, float speed, float distance,
            ref float distanceTraveled, float acceleration, float ogSpeed, float maxSpeed, Animator animator)
        {
            const string walkKey = "walk";
            const string runKey = "run";
            
            Vector3 lookDirection = originalPos - transform.position;

            lookDirection.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
            
            animator.SetBool(walkKey, false);
            animator.SetBool(runKey, true);
            
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
    }
}