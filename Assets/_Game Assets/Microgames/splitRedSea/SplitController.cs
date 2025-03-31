using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.splitRedSea
{
    public class SplitController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform splitter;
        [SerializeField] private Camera mainCamera;
        
        [Header("Settings")]
        [SerializeField] private float grabSplitterRadius;
        [SerializeField] private float speed;
        [SerializeField] private float splitterXPosition;
        [SerializeField] private Vector2 minMaxSplitterYPosition;
        private float splitterProgressPosition;
        
        [Header("Events")]
        [SerializeField] private UnityEvent startedSplittingUnityEvent;
        [SerializeField] private UnityEvent stoppedSplittingUnityEvent;
        [SerializeField] private UnityEvent finishedSplittingUnityEvent;
        
        private bool isGrabbing;
        private bool allowInput;

        private void Start()
        {
            splitter.position = new Vector2(splitterXPosition, minMaxSplitterYPosition.x);
            isGrabbing = false;
            allowInput = true;
            
            splitter.GetChild(0).gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!allowInput) return;
            
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            if (!isGrabbing && Input.GetMouseButtonDown(0))
            {
                if (Vector2.Distance(splitter.position, mousePosition) < grabSplitterRadius)
                {
                    isGrabbing = true;
                    splitter.GetChild(0).gameObject.SetActive(true);
                    startedSplittingUnityEvent?.Invoke();
                }
            }

            mousePosition.x = splitterXPosition;
            
            if (isGrabbing && Input.GetMouseButton(0))
            {
                Vector3 newPosition = new Vector3(splitter.position.x, Mathf.Lerp(splitter.position.y, mousePosition.y, Time.deltaTime * speed), splitter.position.z);
                newPosition.y = Mathf.Clamp(newPosition.y, minMaxSplitterYPosition.x, minMaxSplitterYPosition.y);
                splitter.position = newPosition;

                if (newPosition.y > minMaxSplitterYPosition.y - 0.01f)
                {
                    allowInput = false;

                    splitter.DOMoveY(minMaxSplitterYPosition.y + 15f, 0.65f);
                    finishedSplittingUnityEvent?.Invoke();
                }
            }
            
            if (isGrabbing && Input.GetMouseButtonUp(0))
            {
                isGrabbing = false;
                stoppedSplittingUnityEvent?.Invoke();
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.black;
            UnityEditor.Handles.Label(splitter.position, $"{splitter.position.y}");
        }
        #endif
    }
}
