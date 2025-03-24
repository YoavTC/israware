using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.rhythm
{
    public class EndDetector : MonoBehaviour
    {
        [SerializeField] private UnityEvent failUnityEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NotPlayer"))
            {
                failUnityEvent?.Invoke();
            }
        }
    }
}
