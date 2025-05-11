using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.defeatAdolf.Code
{
    public class HealthManager : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float playerHealth;
        [SerializeField] private float enemyHealth;
        
        [SerializeField] private Vector2 damageRange;
        [SerializeField] private Vector2 healRange;

        [SerializeField, HideInEditMode] private float globalDamageMultiplier;
        [SerializeField] private float globalDamageMultiplierStep;
        
        private float maxPlayerHealth;
        private float maxEnemyHealth;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<float> playerHealthInitializedUnityEvent;
        [SerializeField] private UnityEvent<float> enemyHealthInitializedUnityEvent;
        [SerializeField] private UnityEvent<float> playerHealthChangedUnityEvent;
        [SerializeField] private UnityEvent<float> enemyHealthChangedUnityEvent;
        [Space]
        [SerializeField] private UnityEvent<bool> gameOverUnityEvent;

        private void Start()
        {
            globalDamageMultiplier = 1f;
            
            maxPlayerHealth = playerHealth;
            maxEnemyHealth = enemyHealth;
            
            playerHealthInitializedUnityEvent?.Invoke(playerHealth);
            enemyHealthInitializedUnityEvent?.Invoke(enemyHealth);
        }

        public void DamagePlayer(float multiplier)
        {
            playerHealth -= (Random.Range(damageRange.x, damageRange.y) * multiplier) * globalDamageMultiplier;
            playerHealthChangedUnityEvent?.Invoke(playerHealth);
            IncreaseGlobalDamageMultiplier();
            
            if (playerHealth <= 0) gameOverUnityEvent?.Invoke(false);
        }
        
        public void DamageEnemy(float multiplier)
        {
            enemyHealth -= (Random.Range(damageRange.x, damageRange.y) * multiplier) * globalDamageMultiplier;
            enemyHealthChangedUnityEvent?.Invoke(enemyHealth);
            IncreaseGlobalDamageMultiplier();
            
            if (enemyHealth <= 0) gameOverUnityEvent?.Invoke(true);
        }
        
        private void IncreaseGlobalDamageMultiplier()
        {
            globalDamageMultiplier += globalDamageMultiplierStep;
        }

        public void HealPlayer()
        {
            playerHealth += Random.Range(healRange.x, healRange.y);
            playerHealth = Mathf.Clamp(playerHealth, 0, maxPlayerHealth);
            playerHealthChangedUnityEvent?.Invoke(playerHealth);
        }
        
        public void HealEnemy()
        {
            enemyHealth += Random.Range(healRange.x, healRange.y);
            enemyHealth = Mathf.Clamp(enemyHealth, 0, maxEnemyHealth);
            enemyHealthChangedUnityEvent?.Invoke(enemyHealth);
        }
    }
}
