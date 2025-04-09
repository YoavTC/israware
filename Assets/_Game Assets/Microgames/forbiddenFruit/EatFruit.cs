using System;
using System.Collections;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.forbiddenFruit
{
    public class EatFruit : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Camera mainCamera;
        
        [Header("Settings")]
        [SerializeField] private float fruitSizeRadius;
        [SerializeField] private Vector2 fruitPosition;

        [Header("Events")]
        [SerializeField] private UnityEvent eatFruitUnityEvent;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(fruitPosition, mousePos) <= fruitSizeRadius)
                {
                    eatFruitUnityEvent?.Invoke();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Gizmos.color = Vector2.Distance(fruitPosition, mousePos) <= fruitSizeRadius ? 
                Color.green : Color.red;
            
            Gizmos.DrawLine(fruitPosition, mainCamera.ScreenToWorldPoint(Input.mousePosition));
            
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(fruitPosition, fruitSizeRadius);
        }
    }
}
