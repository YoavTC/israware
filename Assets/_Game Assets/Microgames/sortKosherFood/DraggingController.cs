using System;
using System.Linq;
using DG.Tweening;
using External_Packages.Extra_Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;

namespace _Game_Assets.Microgames.sortKosherFood
{
    public class DraggingController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float draggedObjectZOffset = 10f;
        [SerializeField] private float throwingDistanceThreshold;
        private Vector2 mousePos;
        
        private Transform currentDraggedObject;
        private Vector2 draggedObjectOffset;

        [SerializeField] private float outTransitionDuration;
        [SerializeField] private Vector2[] outPositions;

        [SerializeField] private UnityEvent onThrowUnityEvent;

        [SerializeField] private float grabEffectScale;
        [SerializeField] private float grabEffectDuration;
        private float grabbedObjectInitialScale;

        private void Update()
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            HandleCursor();
            HandleDragging();
        }

        private void HandleDragging()
        {
            // Start dragging
            if (Input.GetMouseButtonDown(0) && currentDraggedObject == null)
            {
                currentDraggedObject = GetDraggedObject();
            }
            
            if (currentDraggedObject == null) return;

            // Move dragged object
            if (Input.GetMouseButton(0))
            {
                currentDraggedObject.position = mousePos.WithZ(draggedObjectZOffset) + (Vector3) draggedObjectOffset;
            }
            
            // Drop dragged object
            if (Input.GetMouseButtonUp(0))
            {
                TryThrow(currentDraggedObject);
                DropDraggedObject();
            }
        }

        private Transform GetDraggedObject()
        {
            Collider2D targetObject = Physics2D.OverlapCircle(mousePos, 0.1f);
            if (targetObject != null )
            {
                Debug.Log("Grabbed");
                GrabEffect(targetObject.transform.GetComponentInChildren<SpriteRenderer>().transform, true);
                draggedObjectOffset = targetObject.transform.position - (Vector3) mousePos;
                targetObject.transform.DOKill(false);
                return targetObject.transform;
            }
            
            return null;
        }

        private void DropDraggedObject()
        {
            if (currentDraggedObject != null)
            {
                currentDraggedObject.transform.position = mousePos + draggedObjectOffset;
                GrabEffect(currentDraggedObject.GetComponentInChildren<SpriteRenderer>().transform, false);
            }

            currentDraggedObject = null;
            draggedObjectOffset = Vector2.zero;
            
            Debug.Log("Dropped");
        }

        private void TryThrow(Transform targetObject)
        {
            if (Mathf.Abs(targetObject.position.x) > throwingDistanceThreshold)
            {
                Debug.Log($"Throwing {targetObject.name}");
                Vector2 outPosition = Mathf.Sign(targetObject.position.x) > 0 ? outPositions[0] : outPositions[1];
                // Vector2 outPosition = outPositions
                //     .OrderBy(position => Vector2.Distance(targetObject.position, position))
                //     .First();
                
                targetObject.GetComponent<Collider2D>().enabled = false;
                targetObject.DOMove(outPosition.With(y:targetObject.position.y * 2f), outTransitionDuration)
                    .OnComplete(() => onThrowUnityEvent?.Invoke());
            } else {
                Debug.Log($"Returning {targetObject.name}");
                targetObject.DOMove(Vector3.zero, outTransitionDuration);
            }
        }

        private void GrabEffect(Transform targetTransform, bool pickingUp)
        {
            targetTransform.DOKill(true);
            if (pickingUp) grabbedObjectInitialScale = targetTransform.localScale.x;
            targetTransform.DOScale(pickingUp ? targetTransform.localScale.x * grabEffectScale : grabbedObjectInitialScale, grabEffectDuration);
        }

        private void HandleCursor()
        {
        }
    }
}
