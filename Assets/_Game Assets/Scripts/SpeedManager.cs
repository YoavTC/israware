using _Game_Assets.Scripts.Definitions;
using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class SpeedManager : MonoBehaviour, IMicrogameCallbacksListener
    {
        [Header("Speed Settings")]
        [SerializeField, ReadOnly] private float currentSpeed;
        [SerializeField, ReadOnly] private float normalSpeed;
        [Space]
        [SerializeField] private float speedStep;

        private void Start()
        {
            currentSpeed = 1f;
            normalSpeed = Time.timeScale;
        }

        public void OnMicrogameLoaded(MicrogameScriptableObject microgame)
        {
            Time.timeScale = microgame.isBossLevel ? normalSpeed : currentSpeed;
        }

        public void OnMicrogameFinished(bool result)
        {
            Time.timeScale = normalSpeed;
            if (result) AdvanceSpeed();
        }
        
        private void AdvanceSpeed()
        {
            currentSpeed += speedStep;
        }
    }
}