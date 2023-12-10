using UnityEngine;
using UnityEngine.Serialization;

namespace Movements
{
    [CreateAssetMenu(fileName = "movement", menuName = "movements/Random")]
    public class RandomDirMovement : Movement
    {
        [SerializeField] private float maxTime = 4;

        private float timer;
        private int routine;
        private Quaternion direction;
        private float angle;

        public override void Move(Transform transform, Vector3 originalPos, ref bool direction, float speed,
            float distance, ref float distanceTraveled, float acceleration, float ogSpeed, float maxSpeed, 
            Animator animator)
        {
            const string walkKey = "walk";
            const string runKey = "run";
            
            animator.SetBool(runKey, false);
            
            timer += Time.deltaTime;

            if (timer > maxTime)
            {
                routine = Random.Range(0, 2);
                timer = 0;
            }


            switch (routine)
            {
                case 0:
                    animator.SetBool(walkKey, false);
                    break;

                case 1:
                    angle = Random.Range(0, 360);
                    this.direction = Quaternion.Euler(0, angle, 0);
                    routine++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, this.direction, speed);
                    transform.Translate(Vector3.forward * Time.deltaTime);
                    animator.SetBool(walkKey, true);
                    break;
            }
        }
    }
}