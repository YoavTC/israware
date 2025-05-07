using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using External_Packages.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.sortKosherFood
{
    public class FoodHandler : MonoBehaviour
    {
        [SerializeField] private int foodsCount;
        
        [SerializedDictionary("Food", "Is Kosher"), SerializeField] 
        private SerializedDictionary<GameObject, bool> foodsKosherDictionary;
        private Dictionary<Transform, bool> instantiatedFoodsKosherDictionary;
        
        [SerializeField] private Vector2 foodSpawnPoint;
        [SerializeField] private float transitionSpeed;
        
        [SerializeField] private Transform[] foods;
        private int currentFoodIndex;
        
        [SerializeField] private UnityEvent<bool> finishedFoodsUnityEvent;

        private IEnumerator Start()
        {
            var randomFoods = GetRandomFoods(foodsCount);
            foods = InstantiateFoods(randomFoods);
            currentFoodIndex = 0;
            
            yield return new WaitForSeconds(0.1f);
            
            TransitionNextFood();
        }

        private Transform[] GetRandomFoods(int count)
        {
            if (count >= foodsKosherDictionary.Count)
            {
                return foodsKosherDictionary.Keys.Select(food => food.transform).ToArray();
            }
            
            GameObject[] availableFoods = foodsKosherDictionary.Keys.ToArray();
            
            // Use Durstenfeld implementation of the Fisher-Yates shuffle for truly random selection
            availableFoods.Shuffle();

            return availableFoods
                .Take(count)
                .Select(food => food.transform)
                .ToArray();
        }

        private Transform[] InstantiateFoods(Transform[] foodsToInstantiate)
        {
            List<Transform> instantiatedFoods = new List<Transform>();
            instantiatedFoodsKosherDictionary = new Dictionary<Transform, bool>();
            
            foreach (Transform food in foodsToInstantiate)
            {
                var foodInstance = Instantiate(food.gameObject, foodSpawnPoint, Quaternion.identity, transform);
                
                instantiatedFoodsKosherDictionary.Add(foodInstance.transform, foodsKosherDictionary[food.gameObject]);
                instantiatedFoods.Add(foodInstance.transform);
            }

            return instantiatedFoods.ToArray();
        }

        private void TransitionNextFood()
        {
            if (currentFoodIndex < foods.Length)
            {
                Debug.Log($"Moving food {foods[currentFoodIndex].gameObject.name}");
                foods[currentFoodIndex].gameObject.transform.DOMove(Vector3.zero, transitionSpeed);
                foods[currentFoodIndex].EnableChildren();
                currentFoodIndex++;
            }
            else
            {
                finishedFoodsUnityEvent?.Invoke(true);
            }
        }

        public void OnFoodThrown(Transform food, bool kosher)
        {
            Debug.Log($"Food thrown as {(kosher ? "kosher" : "not kosher")}");
            bool isKosher = instantiatedFoodsKosherDictionary[food];
            Debug.Log($"Thrown food {food.name} got {(kosher ? "kosher" : "not kosher")} -- is {(isKosher ? "kosher" : "not kosher")}");
            if (isKosher != kosher)
            {
                finishedFoodsUnityEvent?.Invoke(false);
            } else TransitionNextFood();
        }
    }
}
