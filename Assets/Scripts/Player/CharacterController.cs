using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player
{
    /// <summary>
    /// Class that enables a character to be moved
    /// </summary>
    public class CharacterController : MonoBehaviour
    {
        private const int MaxFloorDistance = 10;

        [Header("Setup")] [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private Transform feetPivot;

        [Header("Movement")] [SerializeField] private float movementSpeed = 10f;

        [SerializeField] private float jumpForce = 10f;

        [SerializeField] private float minJumpDistance = 0.25f;

        [SerializeField] private float jumpBufferTime = 0.25f;

        private Vector3 _currentMovement;
        private Coroutine _jumpCoroutine;

        private bool isSprinting = false;

        private bool _isJumpInput;
        [SerializeField] private float coyoteTime;

        private void OnValidate()
        {
            rigidBody ??= GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (!rigidBody)
            {
                //<color=xxx> nos permite colorear una string
                //mas data sobre las string con $ (string interpolation):
                //https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/tokens/interpolated
                Debug.LogError($"<color=grey>{name}:</color> {nameof(rigidBody)} is null!" +
                               $"\n<color=red>Disabling this component to avoid NullReferences!</color>");
                enabled = false;
            }

            if (!feetPivot)
            {
                Debug.LogWarning($"<color=grey>{name}:</color> {nameof(feetPivot)} is null!");
            }
        }

        private void FixedUpdate()
        {
            float playerSpeed = isSprinting ? movementSpeed * 2 : movementSpeed;

            rigidBody.velocity = _currentMovement * playerSpeed + Vector3.up * rigidBody.velocity.y;
        }

        /// <summary>
        /// moves the character by walking
        /// </summary>
        public void OnMove(InputValue context)
        {
            var movementInput = context.Get<Vector2>();
            _currentMovement = new Vector3(-movementInput.y, 0, movementInput.x);

            Quaternion meshRotation = rigidBody.transform.rotation;
            _currentMovement = meshRotation * _currentMovement;
        }

        public void OnSprint(InputValue value)
        {
            isSprinting = value.isPressed;
        }

        /// <summary>
        /// Makes the character jump
        /// </summary>
        public void OnJump()
        {
            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);
            _jumpCoroutine = StartCoroutine(JumpCoroutine());
        }

        public void OnShoot()
        {
            GetComponent<ShootRaycast>().Shoot();
        }

        public void OnShootSecondary()
        {
            GetComponent<ShootInstance>().Fire();
        }

        public void OnLockTarget()
        {
            Debug.Log("LockTarget");
            Transform nearestTarget = GetComponent<CameraManager>().FindNearestTarget(transform.position, 20f);

            if (nearestTarget != null)
            {
                rigidBody.transform.LookAt(nearestTarget);  
            }
        
        }

        /// <summary>
        /// Runs the characters Jump as soon as it's close to the ground and in the fixedUpdate period.
        /// </summary>
        /// <returns></returns>
        private IEnumerator JumpCoroutine()
        {
            //Siempre hacer sanity checks. Hoy en dia, las CPUs son lo suficientemente avanazadas para no verse
            //gravemente afectadas por los sanity checks o nullChecks
            if (!feetPivot)
                yield break;

            //Podemos utilizar for en reemplazo del While.
            //Ambos funcionan de la misma manera y su unica diferencia es como se ven.

            //var timeElapsed = 0.0f;
            //while (timeElapsed <= jumpBufferTime)
            for (var timeElapsed = 0.0f; timeElapsed <= jumpBufferTime; timeElapsed += Time.fixedDeltaTime)
            {
                yield return new WaitForFixedUpdate();
                var isFalling = rigidBody.velocity.y <= 0;
                var currentFeetPosition = feetPivot.position;
                //            X0                  =         Xf          -       velocity     *   time

                var canNormalJump = isFalling && CanJumpInPosition(currentFeetPosition);

                var coyoteTimeFeetPosition = currentFeetPosition - rigidBody.velocity * coyoteTime;
                var canCoyoteJump = isFalling && CanJumpInPosition(coyoteTimeFeetPosition);

                if (!canNormalJump && canCoyoteJump)
                {
                    Debug.DrawLine(currentFeetPosition, coyoteTimeFeetPosition, Color.cyan, 5f);
                }

                if (canNormalJump || canCoyoteJump)
                {
                    var jumpForceVector = Vector3.up * jumpForce;

                    //Esto cancela la velocidad de caida.
                    if (rigidBody.velocity.y < 0)
                        jumpForceVector.y -= rigidBody.velocity.y;

                    rigidBody.AddForce(jumpForceVector, ForceMode.Impulse);

                    //if (timeElapsed > 0)
                    //Debug.Log($"<color=grey>{name}: buffered jump for {timeElapsed} seconds</color>");

                    yield break;
                }
            }
        }

        /// <summary>
        /// Checks if the character is able to jump when in a certain position.
        /// </summary>
        /// <param name="currentFeetPosition"></param>
        /// <returns></returns>
        private bool CanJumpInPosition(Vector3 currentFeetPosition)
        {
            //La variable hit puede ser presentada dirertamente en la llamada al metodo Raycast
            //La keyword out significa que le damos acceso al metodo para asignarle un valor a nuestra variable.
            //Ojo! El valor con el que termina podria ser nulo en otros metodos, pero en el caso del raycast, nunca sera asi
            return Physics.Raycast(currentFeetPosition, Vector3.down, out var hit, MaxFloorDistance)
                   && hit.distance <= minJumpDistance;
        }

        private void OnDrawGizmosSelected()
        {
            if (!feetPivot)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(feetPivot.position, feetPivot.position + Vector3.down * minJumpDistance);
        }
    }
}