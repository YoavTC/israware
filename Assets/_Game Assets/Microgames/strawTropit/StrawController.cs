using System;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.strawTropit
{
    public class StrawController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform strawTransform;
        [SerializeField] private Vector2 strawPokingPoint;

        [Header("Settings")]
        [SerializeField] private float zPosition;
        [SerializeField] private float pressYOffset;

        [Header("Poking Point")]
        [SerializeField] private Vector2 pokingPoint;
        [SerializeField] private float pokingPointRadius;

        [Header("Events")]
        [SerializeField] private UnityEvent correctPokeEvent;
        
        private Vector3 mousePosition;
        private bool allowPoking = true;
        
        void Update()
        {
            if (allowPoking)
            {
                MoveStraw();
                HandleInput();
            }
        }

        private void MoveStraw()
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = zPosition;
            
            strawTransform.position = mousePosition;
        }
        
        private void HandleInput()
        {
            bool isPressing = Input.GetMouseButton(0);
            
            Vector3 targetPosition = strawTransform.position;
            targetPosition.y = isPressing ? mousePosition.y + pressYOffset : strawTransform.position.y;

            strawTransform.position = targetPosition;

            if (isPressing && Vector2.Distance((mousePosition + (Vector3)strawPokingPoint) + new Vector3(0f, pressYOffset, 0f), pokingPoint) < pokingPointRadius)
            {
                correctPokeEvent?.Invoke();
                allowPoking = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pokingPoint, mousePosition + (Vector3) strawPokingPoint);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pokingPoint, pokingPointRadius);
        }
    }
}
