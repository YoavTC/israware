using System;
using _Game_Assets.Scripts.Definitions;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game_Assets.Scripts
{
    public class Timer : MonoBehaviour, IMicrogameCallbacksListener
    {
        [SerializeField] private RectTransform rectTransform;
        
        [Header("Sliders")]
        [SerializeField] private Slider leftSlider;
        [SerializeField] private Slider rightSlider;
        [SerializeField] private float easingFactor = 0.57f;
        
        [Header("Time")]
        [SerializeField] private bool timerActive;
        [SerializeField] private float time;
        [SerializeField, ReadOnly] private float originalTime;
        [SerializeField] private UnityEvent<bool> timerFinishedUnityEvent;
        private bool winAtFinish;
        
        public void OnMicrogameLoaded(MicrogameScriptableObject microgame)
        {
            if (microgame.maxMicrogameTime < 0) DisableTimer();
            else
            {
                time = microgame.maxMicrogameTime;
                originalTime = time;

                winAtFinish = microgame.winAtTimerFinish;
                timerActive = true;
                
                ToggleTimerVisibility(true);
            }
        }

        public void OnMicrogameFinished(bool result)
        {
            DisableTimer();
        }
        
        private void Update()
        {
            if (!timerActive) return;

            time -= Time.deltaTime;
            AdjustSliders(time / originalTime);
            
            if (time <= 0f)
            {
                timerFinishedUnityEvent?.Invoke(winAtFinish);
                timerActive = false;
            }
        }
        
        private void AdjustSliders(float progress)
        {
            float easedProgress = progress > 0 ? Mathf.Pow(progress, easingFactor) : 0f;
            leftSlider.value = easedProgress;
            rightSlider.value = easedProgress;
        }

        public void DisableTimer()
        {
            timerActive = false;
            ToggleTimerVisibility(false);
        }
        
        private void ToggleTimerVisibility(bool state)
        {
            rectTransform.DOAnchorPosY(state ? 0f : -50f, 0);
        }
    }
}
