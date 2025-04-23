using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class AnimatorEventsNotifier : MonoBehaviour
    { 
        [SerializeField] private UnityEvent jumpUnityEvent;
        [SerializeField] private UnityEvent slideUnityEvent;
        
        public void OnJump() => jumpUnityEvent?.Invoke();
        public void OnSlide() => slideUnityEvent?.Invoke();
    }
}