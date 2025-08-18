using System.Collections;
using _Game_Assets.Microgames.defeatAdolf.Code.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.defeatAdolf.Code
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Vector2 randomChoosingDurationRange;
        [SerializeField, Range(0, 200)] private int attemptHealingThreshold;
        [SerializeField, Range(0f, 1f)] private float healChancePercentage;
        
        [SerializeField] private UnityEvent<ActionType> actionChosenUnityEvent;

        private float currentHealth;
        
        public IEnumerator GetEnemyAction()
        {
            bool randomAttackType = External_Packages.Random.RandomBool();
            ActionType actionChosen = randomAttackType ? ActionType.ENEMY_MELEE_ATTACK : ActionType.ENEMY_RANGED_ATTACK;
            
            float enemyHealth = GetEnemyHealth();
            if (enemyHealth <= attemptHealingThreshold)
            {
                if (Random.value <= healChancePercentage)
                {
                    actionChosen = ActionType.ENEMY_HEAL;
                }
            }
            
            // Simulate a delay for the enemy to choose an action
            yield return new WaitForSeconds(Random.Range(randomChoosingDurationRange.x, randomChoosingDurationRange.y));
            
            actionChosenUnityEvent?.Invoke(actionChosen);
        }

        public void UpdateEnemyHealth(float health)
        {
            currentHealth = health;
        }
        
        private float GetEnemyHealth()
        {
            return currentHealth;
        }
    }
}
