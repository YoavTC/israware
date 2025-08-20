using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using External_Packages.Extensions;
using External_Packages.Extra_Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.makeCoffee
{
    public class SpoonController : MonoBehaviour
    {
        [Header("Debug")] 
        [SerializeField] private bool usedSugar;
        [SerializeField] private bool usedCoffee;
        [SerializeField] private bool usedWater;
        
        [SerializeField] private SpoonItem currentItem;
        
        [Header("Components")] 
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform spoonTransform;
        [SerializeField] private SpriteRenderer spoonItemSpriteRenderer;
        [SerializeField] private GameObject kettleGameObject;

        [Header("Sprites")] 
        [SerializeField] private SerializedDictionary<Transform, Sprite> itemSprites;
        private Sprite emptySpoonSprite;
        
        [Header("Settings")] [SerializeField] private float positionZOffset;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Vector3 activeRotation;
        private Vector3 inactiveRotation;
        
        [Header("Events")]
        [SerializeField] private UnityEvent usedAnyItemUnityEvent;
        [SerializeField] private UnityEvent usedSugarUnityEvent;
        [SerializeField] private UnityEvent usedCoffeeUnityEvent;
        [SerializeField] private UnityEvent usedWaterUnityEvent;
        [SerializeField] private UnityEvent usedAllUnityEvent;

        private TweenSBobEffect lastInteractedItem;
        private Vector2 lastInteractedItemPosition;

        private void Start()
        {
            inactiveRotation = spoonTransform.rotation.eulerAngles;
            emptySpoonSprite = spoonItemSpriteRenderer.sprite;
        }

        void Update()
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            spoonTransform.position = mousePosition.With(z: positionZOffset);

            if (Input.GetMouseButtonDown(0)) RotateSpoon(true);
            if (Input.GetMouseButtonUp(0)) RotateSpoon(false);
        }

        private void RotateSpoon(bool up)
        {
            spoonTransform.DOKill();
            spoonTransform.DORotate(up ? activeRotation : inactiveRotation, rotationSpeed);
        }
        
        private enum SpoonItem
        {
            NONE,
            SUGAR,
            COFFEE,
            WATER,
        }

        public void ClickedSugar(TweenSBobEffect item)
        {
            if (SelectItem(item))
            {
                currentItem = SpoonItem.SUGAR;
            }
        }

        public void ClickedCoffee(TweenSBobEffect item)
        {
            if (SelectItem(item))
            {
                currentItem = SpoonItem.COFFEE;
            }
        }

        public void ClickedKettle(TweenSBobEffect item)
        {
            if (SelectItem(item))
            {
                currentItem = SpoonItem.WATER;
                kettleGameObject.SetActive(false);
            }
        }

        public void ClickedGlass(TweenSBobEffect item)
        {
            DeselectLastInteractedItem();

            if (lastInteractedItem != null)
            {
                if (currentItem == SpoonItem.SUGAR && !usedSugar)
                {
                    usedSugar = true;
                    usedSugarUnityEvent?.Invoke();
                }
                
                if (currentItem == SpoonItem.COFFEE && !usedCoffee)
                {
                    usedCoffee = true;
                    usedCoffeeUnityEvent?.Invoke();
                }
                
                if (currentItem == SpoonItem.WATER && !usedWater)
                {
                    usedWater = true;
                    usedWaterUnityEvent?.Invoke();
                }

                if (usedSugar && usedCoffee && usedWater)
                {
                    usedAllUnityEvent?.Invoke();
                }
            }
        }

        private bool SelectItem(TweenSBobEffect item)
        {
            if (item == lastInteractedItem) return false;
            
            DeselectLastInteractedItem();

            lastInteractedItem = item.gameObject.GetOrAdd<TweenSBobEffect>();
            lastInteractedItemPosition = lastInteractedItem.transform.position;
            lastInteractedItem.DoEffect();

            spoonItemSpriteRenderer.sprite = itemSprites[item.transform];
            
            usedAnyItemUnityEvent?.Invoke();

            return true;
        }

        private void DeselectLastInteractedItem()
        {
            kettleGameObject.SetActive(true);
            
            lastInteractedItem?.transform.DOKill();
            lastInteractedItem?.transform.DOMove(lastInteractedItemPosition, 0.5f);

            spoonItemSpriteRenderer.sprite = emptySpoonSprite;
        }
    }
}