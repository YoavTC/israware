using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class Wolter : MonoBehaviour
    {
        [Header("UI Components")] 
        [SerializeField] private UnityEvent<string> collectCoinUnityEvent;

        private int coinsCollected;
        
        public void OnCollectCoin()
        {
            coinsCollected++;
            collectCoinUnityEvent?.Invoke(coinsCollected.ToString());
        }

        public void OnObstacleHit(Transform obstacleTransform)
        {
            Debug.Log("Obstacle hit: " + obstacleTransform.name);
        }
    }
}
