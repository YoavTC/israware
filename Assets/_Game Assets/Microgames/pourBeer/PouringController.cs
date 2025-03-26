using NaughtyAttributes;
using UnityEngine;

namespace _Game_Assets.Microgames.pourBeer
{
    public class PouringController : MonoBehaviour
    {
        [SerializeField, ReadOnly] private float rotationInput;
        [SerializeField] private float rotationSpeed = 50f; // Speed of rotation change per second
        [SerializeField] private float rotationMultiplier = -0.5f;

        [SerializeField] private float pourThresholdAngle = -0.85f;
        [SerializeField] private Vector2Int rotationLimits = new Vector2Int(0, 360);

        void Update()
        {
            if (Input.GetKey(KeyCode.Space)) rotationInput += rotationSpeed * Time.deltaTime;
            else rotationInput -= rotationSpeed * Time.deltaTime;

            rotationInput = Mathf.Clamp(rotationInput,
                Mathf.Min(rotationLimits.x, rotationLimits.y),
                Mathf.Max(rotationLimits.x, rotationLimits.y));

            transform.rotation = Quaternion.Euler(0, 0, rotationInput * rotationMultiplier);

            if (transform.rotation.z < pourThresholdAngle)
            {
                PourBeer();
            }
        }

        private void PourBeer()
        {
            Debug.Log("Pouring Beer");
        }
    }
}