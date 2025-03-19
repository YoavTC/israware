using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityUtils;

namespace _Game_Assets.Microgames.sortKosherFood
{
    public class FoodHandler : MonoBehaviour
    {
        [SerializeField] private float transitionSpeed;
        [SerializeField] private Transform[] foods;
        [SerializeField] private int currentFoodIndex;

        private void Start()
        {
            currentFoodIndex = 0;
            foods = transform.Children().ToArray();
            // foods = transform.Cast<Transform>().Where(child => child != transform).ToArray();
            
            TransitionNextFood();
        }

        public void TransitionNextFood()
        {
            if (currentFoodIndex < foods.Length)
            {
                foods[currentFoodIndex].DOMove(Vector2.zero, transitionSpeed);
                foods[currentFoodIndex].EnableChildren();
                currentFoodIndex++;
            }
        }
    }
}
