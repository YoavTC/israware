using UnityEngine;
using UnityEngine.Events;
using EditorAttributes;

namespace _Game_Assets.Microgames.ripNebuchadnezzar
{
    public class MoveRagdoll : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Rigidbody2D torsoRb;
        [SerializeField] private Rigidbody2D headRb;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject ripBloodParticlePrefab;
        
        [Header("Settings")]
        [SerializeField] private float grabRadius;
        [SerializeField] private float speed;
        [SerializeField] private Bounds mouseBounds;
        
        [SerializeField] private bool removeVelocityOnGrab;
        [SerializeField] private bool removeVelocityOnRelease;
        private bool RemovingVelocity => removeVelocityOnGrab || removeVelocityOnRelease; 
        [SerializeField, EnableField(nameof(RemovingVelocity))] private bool removeChildrenVelocity;
        
        [Header("Events")]
        [SerializeField] private UnityEvent rippedUnityEvent;
        [SerializeField] private UnityEvent jointBrokeUnityEvent;
        
        private Vector2 mousePosition;
        private bool isGrabbing;
        private Rigidbody2D grabbedRb;
        
        void Update()
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            mousePosition.x = Mathf.Clamp(mousePosition.x, mouseBounds.min.x, mouseBounds.max.x);
            mousePosition.y = Mathf.Clamp(mousePosition.y, mouseBounds.min.y, mouseBounds.max.y);
            
            Debug.DrawLine(Vector3.zero, mousePosition, Color.red);
            
            if (Input.GetMouseButtonDown(0))
            {
                var grabbedCollider2D = Physics2D.OverlapCircle(mousePosition, grabRadius, 1 << LayerMask.NameToLayer("Ragdoll"));
                if (grabbedCollider2D != null 
                    && grabbedCollider2D.CompareTag("NotPlayer") 
                    && grabbedCollider2D.TryGetComponent(out grabbedRb))
                {
                    isGrabbing = true;
                    if (removeVelocityOnGrab) RemoveVelocity();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isGrabbing = false;
                if (removeVelocityOnRelease) RemoveVelocity();
            }
        }

        private void FixedUpdate()
        {
            if (!isGrabbing) return;
            Vector2 targetPosition = Vector2.MoveTowards(grabbedRb.position, mousePosition, speed * Time.fixedDeltaTime);
            
            if (isGrabbing)
            {
                grabbedRb.MovePosition(targetPosition);
                Debug.DrawLine(grabbedRb.position, targetPosition, Color.magenta);
            }
        }

        private void RemoveVelocity()
        {
            grabbedRb.linearVelocity = Vector2.zero;
            grabbedRb.angularVelocity = 0;

            if (removeChildrenVelocity)
            {
                foreach (var childRb in grabbedRb.GetComponentsInChildren<Rigidbody2D>())
                {
                    childRb.linearVelocity = Vector2.zero;
                    childRb.angularVelocity = 0f;
                }
            }
        }

        public void JointBreak2D(Rigidbody2D rb)
        {
            if (rb == torsoRb || rb == headRb)
            {
                rippedUnityEvent?.Invoke();
            }
            
            jointBrokeUnityEvent?.Invoke();
            Instantiate(ripBloodParticlePrefab, rb.position, Quaternion.identity);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(mouseBounds.center, mouseBounds.size);
        }
    }
}
