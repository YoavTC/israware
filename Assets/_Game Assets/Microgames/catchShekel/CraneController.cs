using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;

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
            if (!allowMove) return;
            
            HandleInput();
            MoveCrane();
        }

        private void HandleInput()
        {
            if (
                Input.GetButtonDown("Submit") ||
                Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Return) ||
                Input.GetMouseButtonDown(0) ||
                Input.GetMouseButtonDown(1)
                )
            {
                allowMove = false;
                StartCoroutine(DescendCraneCoroutine());
            }
        }

        private void MoveCrane()
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            mousePosition.y = craneYZPosition.x;
            mousePosition.z = craneYZPosition.y;
            
            craneTransform.position = Vector3.Lerp(craneTransform.position, mousePosition, craneMoveSpeed * Time.deltaTime);
        }

        private IEnumerator DescendCraneCoroutine()
        {
            craneStartDescendingUnityEvent?.Invoke();
            yield return craneTransform
                .DOMoveY(craneTransform.position.y - craneDescendDistance, craneDescendSpeed)
                .WaitForCompletion();
            
            craneStopDescendingUnityEvent?.Invoke();
            
            bool caught = CheckCatch();

            yield return new WaitForSeconds(craneAscendDelay);
            
            craneStartAscendingUnityEvent?.Invoke();
            yield return craneTransform.
                DOMoveY(craneTransform.position.y + craneDescendDistance, craneDescendSpeed)
                .WaitForCompletion();

            if (!caught)
            {
                craneStopAscendingUnityEvent?.Invoke();
                allowMove = true;
            } else caughtShekelUnityEvent?.Invoke();
        }

        private bool CheckCatch()
        {
            if (Vector2.Distance(shekel.ShekelTransform.position, craneTransform.position + craneGrabPositionOffset) < grabRadius)
            {
                shekel.ShekelTransform.SetParent(craneTransform);
                shekel.enabled = false;
                
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
