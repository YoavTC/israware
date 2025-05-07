using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using External_Packages.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace _Game_Assets.Microgames.spotTheDifferences
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform pointsParent;
        [SerializeField] private Transform imageHolder;
        
        [Header("Images")]
        [SerializeField] private ImageHandler[] images;
        
        [Header("Raycasting Components")]
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private EventSystem eventSystem;
        
        [Header("Transition Settings")]
        [SerializeField] private GameObject pointCirclePrefab;
        [SerializeField] private Ease circleTransitionEase;
        [SerializeField] private float circleTransitionDuration;
        [SerializeField] private Vector2 circleTransitionStartEndScale;
        
        [Header("Events")]
        [SerializeField] private UnityEvent circleLandUnityEvent;
        [SerializeField] private UnityEvent spottedAllPointsUnityEvent;
        
        private PointerEventData pointerEventData;
        private int targetLayer;
        private List<RaycastResult> results = new List<RaycastResult>();

        void Awake()
        {
            pointerEventData = new PointerEventData(eventSystem);
            targetLayer = LayerMask.NameToLayer("Target");
            
            pointsParent = Instantiate(images[Random.Range(0, images.Length)], imageHolder).PointsParent;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && GetClickedPoint(out Transform point))
            {
                SpotPoint(point);
            }
        }

        private Transform GetClickedPoint(out Transform point)
        {
            results.Clear();
            
            pointerEventData.position = Input.mousePosition;
            graphicRaycaster.Raycast(pointerEventData, results);
            
            point = null;
            
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == targetLayer)
                {
                    point = results[i].gameObject.transform;
                    break;
                }
            }
            
            return point;
        }

        private void SpotPoint(Transform point)
        {
            Transform pointsHolder = point.parent;
                
            foreach (Transform pointChild in pointsHolder.Children())
            {
                Transform circle = Instantiate(pointCirclePrefab, pointChild.position, Quaternion.identity, imageHolder).transform;
                    
                circle.localScale = circleTransitionStartEndScale.x * pointChild.localScale;
                circle.GetComponent<Image>().DOFade(1f, circleTransitionDuration).SetEase(circleTransitionEase);
                circle.DOScale(circleTransitionStartEndScale.y * pointChild.localScale, circleTransitionDuration)
                    .SetEase(circleTransitionEase)
                    .OnComplete(() => circleLandUnityEvent?.Invoke());
            }
            
            pointsHolder.gameObject.SetActive(false);

            if (!pointsParent.Children().Any(child => child.gameObject.activeSelf))
            {
                spottedAllPointsUnityEvent?.Invoke();
            }
        }
    }
}