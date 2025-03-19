using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
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
                draggedObjectOffset = targetObject.transform.position - (Vector3) mousePos;
                return targetObject.transform;
            }
            
            return null;
        }

        private void DropDraggedObject()
        {
            if (currentDraggedObject != null)
            {
                currentDraggedObject.transform.position = mousePos + draggedObjectOffset;
            }

            currentDraggedObject = null;
            draggedObjectOffset = Vector2.zero;
            
            Debug.Log("Dropped");
        }

        private void TryThrow(Transform targetObject)
        {
            if (Vector2.Distance(targetObject.position, Vector2.zero) > throwingDistanceThreshold)
            {
                
                Vector2 outPosition = outPositions
                    .OrderBy(position => Vector2.Distance(targetObject.position, position))
                    .First();
                
                targetObject.DOMove(outPosition, outTransitionDuration);
            }
        }

        private void HandleCursor()
        {
        }
    }
}
