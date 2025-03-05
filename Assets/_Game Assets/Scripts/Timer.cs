using External_Packages.MonoBehaviour_Extensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game_Assets.Scripts
{
    public class Timer : Singleton<Timer>
    {
        [Header("Sliders")]
        [SerializeField] private Slider leftSlider;
        [SerializeField] private Slider rightSlider;
        [SerializeField] private float easingFactor = 0.57f;
        
        [Header("Time")]
        [SerializeField] private bool timerActive;
        [SerializeField] private float time;
        [SerializeField, ReadOnly] private float originalTime;
        [SerializeField] private UnityEvent TimerFinishedUnityEvent;
        
        public void StartTimer(float newTime)
        {
            time = newTime;
            originalTime = time;
            timerActive = true;
        }

        private void Update()
        {
            if (!timerActive) return;

            time -= Time.deltaTime;
            AdjustSliders(time / originalTime);
            
            if (time <= 0f)
            {
                TimerFinishedUnityEvent?.Invoke();
                timerActive = false;
            }
        }
        
        private void AdjustSliders(float progress)
        {
            float easedProgress = progress > 0 ? Mathf.Pow(progress, easingFactor) : 0f;
            leftSlider.value = easedProgress;
            rightSlider.value = easedProgress;
        }

        public void DisableTimer() => timerActive = false;

        #if UNITY_EDITOR
        [Button]
        public void TestTimer10s() => StartTimer(10f);
        #endif
    }
}
