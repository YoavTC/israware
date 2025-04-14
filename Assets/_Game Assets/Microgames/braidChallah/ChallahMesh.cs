using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.braidChallah
{
    public class ChallahMesh : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform[] pinchPoints;

        [Header("Pinching Settings")] 
        [SerializeField] private Transform currentPinchPoint;
        [SerializeField] private float grabPointRadius;

        [Header("Position")]
        private Vector2 pinchStartPosition;
        private Vector2 pinchOffset;
        
        private Vector2 OffsetPosition => mousePosition - pinchOffset;
        private Vector2 mousePosition;

        [Header("Distance")]
        [SerializeField] private float maxPinchDistance;
        [SerializeField] private float pinchingOverclockExtraLifetime;
        private float pinchingOverclockLifetime;
        
        [Header("Overclock")]
        [SerializeField] private float pinchingOverclock;
        [SerializeField] private AnimationCurve pinchingOverclockCurve;
        [SerializeField] private float pinchingOverclockCurveProgress;
        [SerializeField] private float pinchingOverclockStep;
        
        [Header("Events")]
        [SerializeField] private UnityEvent startPinchingUnityEvent;
        [SerializeField] private UnityEvent stopPinchingUnityEvent;

        private void Update()
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Start dragging
            if (Input.GetMouseButtonDown(0))
            {
                Transform point = GetPoint(mousePosition);
                if (point)
                {
                    StartPinching(point);
                }
            }

            // Drag
            if (Input.GetMouseButton(0) && currentPinchPoint)
            {
                MovePinchedPoint();
            }

            // End dragging
            if (Input.GetMouseButtonUp(0) && currentPinchPoint)
            {
                StopPinching();
            }
        }

        private void StartPinching(Transform point)
        {
            startPinchingUnityEvent?.Invoke();
            
            currentPinchPoint = point;
            pinchOffset = mousePosition - (Vector2) point.position;
            pinchStartPosition = OffsetPosition;

            // Reset distance & overclocking
            pinchingOverclock = 1f;
            pinchingOverclockCurveProgress = 0;
            pinchingOverclockLifetime = 0f;
        }

        private void MovePinchedPoint()
        {
            Vector2 pos = OffsetPosition;

            // Clamp the position to the max pinch distance
            if (Vector2.Distance(pinchStartPosition, OffsetPosition) > maxPinchDistance * pinchingOverclock)
            {
                pos = pinchStartPosition + (OffsetPosition - pinchStartPosition).normalized * (maxPinchDistance * pinchingOverclock);

                // Increase overclocking
                pinchingOverclock = pinchingOverclockCurve.Evaluate(pinchingOverclockCurveProgress);
                pinchingOverclockCurveProgress += pinchingOverclockStep;
            }
            
            // Set the position
            currentPinchPoint.position = pos;

            // Calculate extra lifetime
            if (pinchingOverclock >= pinchingOverclockCurve.Evaluate(1f))
            {
                pinchingOverclockLifetime += Time.deltaTime;
                if (pinchingOverclockLifetime >= pinchingOverclockExtraLifetime)
                {
                    StopPinching();
                }
            }
        }

        private void StopPinching()
        {
            stopPinchingUnityEvent?.Invoke();
            
            currentPinchPoint = null;
        }

        private Transform GetPoint(Vector2 pos)
        {
            Transform closestPoint = null;
            float closestDistance = float.MaxValue;

            for (var i = 0; i < pinchPoints.Length; i++)
            {
                Transform point = pinchPoints[i];
                float distance = Vector2.Distance(pos, point.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }

        private void OnDrawGizmosSelected()
        {
            if (pinchPoints.Length < 2) return;
            
            // Draw pinch radius
            for (int i = 0; i < pinchPoints.Length; i++)
            {
                if (currentPinchPoint == pinchPoints[i])
                {
                    Gizmos.color = Color.yellow;
                } else Gizmos.color = Vector2.Distance(mousePosition, pinchPoints[i].position) < grabPointRadius ? Color.green : Color.red;
                
                Gizmos.DrawWireSphere(pinchPoints[i].position, grabPointRadius);
            }
            
            // Draw input
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(mousePosition, grabPointRadius * 0.5f);
            
            // Draw pinch moving
            if (currentPinchPoint)
            {
                UnityEditor.Handles.Label(Vector3.zero, (pinchStartPosition - OffsetPosition).ToString("F2"));
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(pinchStartPosition, OffsetPosition);
            }
        }
    }
}
