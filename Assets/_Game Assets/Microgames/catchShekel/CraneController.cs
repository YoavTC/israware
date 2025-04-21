using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.catchShekel
{
    public class CraneController : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform craneTransform;
        [SerializeField] private ShekelController shekel;
        
        [Header("Move Settings")]
        [SerializeField] private float craneMoveSpeed;
        [SerializeField] private Vector2 craneYZPosition;
        [Space]
        [SerializeField] private float craneDescendDistance;
        [SerializeField] private float craneDescendSpeed;
        [SerializeField] private float craneAscendDelay;
        
        [Header("Rotation Settings")]
        [SerializeField] private float rotationTiltSpeed;
        [SerializeField] private float rotationReturnSpeed;
        [SerializeField] private float minRotationVelocity;

        [Header("Grab Settings")]
        [SerializeField] private float grabRadius;
        [SerializeField] private Vector3 craneGrabPositionOffset;
        
        [Header("Events")]
        [SerializeField] private UnityEvent caughtShekelUnityEvent;
        [SerializeField] private UnityEvent craneStartDescendingUnityEvent;
        [SerializeField] private UnityEvent craneStopDescendingUnityEvent;
        [SerializeField] private UnityEvent craneStartAscendingUnityEvent;
        [SerializeField] private UnityEvent craneStopAscendingUnityEvent;

        
        private Vector3 mousePosition;
        private bool allowMove = true;
        
        void Update()
        {
            if (allowMove)
            {
                HandleInput();
                TiltCrane();
                MoveCrane();
            }
        }

        private void HandleInput()
        {
            // Check for press input
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                allowMove = false;
                StartCoroutine(DescendCraneCoroutine());
            }
        }
        
        private void TiltCrane()
        {
            // Calculate tilt direction
            float dir = mousePosition.x - craneTransform.position.x;
            bool shouldTilt = Mathf.Abs(dir) > minRotationVelocity;
            
            // Assign the tilt direction & speed
            Quaternion targetTiltQuaternion = shouldTilt ? Quaternion.Euler(0f, 0f, -45f * Mathf.Sign(dir)) : Quaternion.Euler(0f, 0f, 0f);
            float targetTiltSpeed = (shouldTilt ? rotationTiltSpeed : rotationReturnSpeed) * Time.deltaTime;

            // Apply rotation
            craneTransform.rotation = Quaternion.Lerp(craneTransform.rotation, targetTiltQuaternion, targetTiltSpeed);
        }
        
        private void MoveCrane()
        {
            // Get mouse position in world space
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            // Reset the Y & Z values to prevent camera clipping
            mousePosition.y = craneYZPosition.x;
            mousePosition.z = craneYZPosition.y;
            
            // Move the crane towards the mouse position
            craneTransform.position = Vector3.Lerp(craneTransform.position, mousePosition, craneMoveSpeed * Time.deltaTime);
        }

        private IEnumerator DescendCraneCoroutine()
        {
            // Correct the rotation
            craneTransform.DORotate(Vector3.zero, 0.2f);
            
            // Lower the crane
            craneStartDescendingUnityEvent?.Invoke();
            yield return craneTransform
                .DOMoveY(craneTransform.position.y - craneDescendDistance, craneDescendSpeed)
                .WaitForCompletion();
            
            craneStopDescendingUnityEvent?.Invoke();
            
            // Check if the shekel was caught
            bool caught = CheckCatch();

            yield return new WaitForSeconds(craneAscendDelay);
            
            // Raise the crane
            craneStartAscendingUnityEvent?.Invoke();
            yield return craneTransform.
                DOMoveY(craneTransform.position.y + craneDescendDistance, craneDescendSpeed)
                .WaitForCompletion();

            // Reset crane if not caught
            if (!caught)
            {
                craneStopAscendingUnityEvent?.Invoke();
                allowMove = true;
            }
        }

        private bool CheckCatch()
        {
            // Check if shekel is close enough to the crane
            if (Vector2.Distance(shekel.ShekelTransform.position, craneTransform.position + craneGrabPositionOffset) < grabRadius)
            {
                // Set the shekel's parent to the crane & disable the shekel's movement
                shekel.ShekelTransform.SetParent(craneTransform);
                shekel.enabled = false;
                caughtShekelUnityEvent?.Invoke();
                
                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(craneTransform.position + craneGrabPositionOffset, grabRadius);
        }
    }
}
