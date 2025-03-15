using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game_Assets.Microgames.throwKeterChair
{
    public class TapSliderController : MonoBehaviour
    {
        [Header("Slider")]
        [SerializeField] private RectTransform progressIndicator;
        [SerializeField] private float indicatorSpeed;
        private float sliderWidth;

        [Header("Validity")]
        [SerializeField] private RectTransform validityIndicator;
        [SerializeField] private float validityThreshold;

        [Header("Events")] 
        [SerializeField] private UnityEvent<bool> stopUnityEvent;

        private void Start()
        {
            validityIndicator.sizeDelta = new Vector2(validityThreshold, validityIndicator.sizeDelta.y);
            
            sliderWidth = GetComponent<RectTransform>().sizeDelta.x;

            progressIndicator.anchoredPosition = new Vector2((sliderWidth / 2f) * -1, 0f);
            progressIndicator.DOAnchorPosX(sliderWidth / 2f, indicatorSpeed).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopSlider();
            }
        }

        private void StopSlider()
        {
            progressIndicator.DOKill(false);
            
            float stopProgressPoint = progressIndicator.anchoredPosition.x;
            bool isValid = Mathf.Abs(stopProgressPoint) <= validityThreshold / 2;
            
            stopUnityEvent?.Invoke(isValid);
        }
    }
}
