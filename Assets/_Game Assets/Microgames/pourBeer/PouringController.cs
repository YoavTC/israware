using DG.Tweening;
using EditorAttributes;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.pourBeer
{
    public class PouringController : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 50f; // Speed of rotation change per second
        [SerializeField] private float rotationMultiplier = -0.5f;
        [SerializeField] private Vector2Int rotationLimits = new Vector2Int(0, 360);
        [SerializeField, ReadOnly] private float rotationInput;
        private bool allowInput = true;

        [Header("Pouring")]
        [SerializeField] private Transform beer;
        [SerializeField, ReadOnly] private bool isPouring;
        [SerializeField] private float beerFillSpeed;
        [SerializeField] private Vector2 minMaxBeerHeight;
        [SerializeField] private Vector2Int pourAngleRange = new Vector2Int(120, 150); // Min and max angles for pouring (in degrees)
        [Space]
        [SerializeField] private MMF_Player pouringFeedback;
        [SerializeField] private LineRenderer pouringLineRenderer;
        
        [Header("Events")]
        [SerializeField] private UnityEvent beerFinishedPouringUnityEvent;

        void Update()
        {
            if (!allowInput) return;
            
            if (Input.GetKey(KeyCode.Space)) rotationInput += rotationSpeed * Time.deltaTime;
            else rotationInput -= rotationSpeed * Time.deltaTime;

            rotationInput = Mathf.Clamp(rotationInput,
                Mathf.Min(rotationLimits.x, rotationLimits.y),
                Mathf.Max(rotationLimits.x, rotationLimits.y));

            transform.rotation = Quaternion.Euler(0, 0, rotationInput * rotationMultiplier);

            // Get the current rotation in degrees (0-360)
            float currentAngle = transform.rotation.eulerAngles.z;
            
            bool isWithinPourRange = IsAngleBetween(currentAngle, Mathf.Min(pourAngleRange.x, pourAngleRange.y), Mathf.Max(pourAngleRange.x, pourAngleRange.y));
            if (isWithinPourRange)
            {
                PourBeer();
            }

            if (isWithinPourRange != isPouring)
            {
                Debug.Log($"Pouring state changed: {isPouring}");
                isPouring = isWithinPourRange;
                OnPouringStateChanged(isPouring);
            }
        }
        
        private bool IsAngleBetween(float angle, float min, float max)
        {
            if (min <= max)
            {
                return angle >= min && angle <= max;
            }
            return angle >= min || angle <= max;
        }

        private void PourBeer()
        {
            Debug.Log("Pouring Beer");
            float newHeight = Mathf.Clamp(beer.localPosition.y + (beerFillSpeed * Time.deltaTime), Mathf.Min(minMaxBeerHeight.x, minMaxBeerHeight.y), Mathf.Max(minMaxBeerHeight.x, minMaxBeerHeight.y));
            beer.localPosition = new Vector3(0, newHeight, 0);
            
            if (newHeight >= minMaxBeerHeight.y)
            {
                allowInput = false;
                
                transform.DORotate(new Vector3(0, 0, 0), 0.75f);
                OnPouringStateChanged(false);
                
                beerFinishedPouringUnityEvent?.Invoke();
            }
        }

        private void OnPouringStateChanged(bool active)
        {
            if (active) pouringFeedback.PlayFeedbacks();
            else pouringFeedback.StopFeedbacks();
            
            pouringLineRenderer.enabled = active;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Draw the threshold angles as lines
            Gizmos.color = Color.red;
            Vector3 minAngleLine = Quaternion.Euler(0, 0, pourAngleRange.x) * Vector3.up;
            Vector3 maxAngleLine = Quaternion.Euler(0, 0, pourAngleRange.y) * Vector3.up;

            Gizmos.DrawLine(transform.position, transform.position + minAngleLine * 2f);
            Gizmos.DrawLine(transform.position, transform.position + maxAngleLine * 2f);

            // Draw the current rotation angle as a line
            Gizmos.color = Color.green;
            float currentAngle = transform.rotation.eulerAngles.z;
            Vector3 currentDirection = Quaternion.Euler(0, 0, currentAngle) * Vector3.up;
            Gizmos.DrawLine(transform.position, transform.position + currentDirection * 2f);

            // Optional: Add a text label to display the current angle in the scene view
            UnityEditor.Handles.Label(Vector3.one * 2, $"{currentAngle:F1}°");
        }
        #endif
    }
}