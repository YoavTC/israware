using System;
using System.Collections.Generic;
using System.Linq;
using _Game_Assets.Scripts.Reusables;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using External_Packages.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game_Assets.Microgames.shoppingList
{
    public class ShoppingList : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Image shoppingListImage;
        
        [Header("Shopping List Items Settings")]
        [SerializeField] private Color untakenItemColor;
        [SerializeField] private Color itemTakenColor;
        [Space]
        [SerializeField] private Transform takingPointTransform; 
        [SerializeField] private TweenSettings takingAnimationSettings;
        
        [Header("Shopping List")]
        [SerializeField] private SerializedDictionary<ShelfItemScriptableObject, Image> shoppingListItemUIDictionary = new SerializedDictionary<ShelfItemScriptableObject, Image>();
        [SerializeField] private List<ShelfItemScriptableObject> takenShelfItems = new List<ShelfItemScriptableObject>();
        
        [Header("Events")]
        [SerializeField] private UnityEvent<ShelfItem> shelfItemTakenUnityEvent;
        [SerializeField] private UnityEvent allShelfItemsTakenUnityEvent;

        public void FillShoppingList(ShelfItemScriptableObject[] items)
        {
            foreach (ShelfItemScriptableObject item in items)
            {
                var itemSlot = new GameObject("ItemSlot");
                itemSlot.transform.SetParent(transform, false);

                itemSlot.GetOrAdd<RectTransform>();
                itemSlot.GetOrAdd<CanvasRenderer>();
                Image itemSlotImage = itemSlot.GetOrAdd<Image>();
                
                itemSlotImage.sprite = item.itemNameSprite;
                itemSlotImage.color = untakenItemColor;
                
                shoppingListItemUIDictionary.Add(item, itemSlotImage);
                
                Debug.Log($"Added {item.name} to shopping list");
            }
        }
        
        public void OnShelfItemTaken(ShelfItem shelfItem)
        {
            // Check if the item is in the shopping list
            if (shelfItem != null && shoppingListItemUIDictionary.Keys.Contains(shelfItem.Item))
            {
                
                // Check if the item is already taken
                if (takenShelfItems.Contains(shelfItem.Item))
                {
                    Debug.Log($"Item already taken: {shelfItem.name}");
                    return;
                }
                
                // Animate the item being taken
                shelfItem.transform.DOMove(takingPointTransform.position, takingAnimationSettings.duration)
                    .SetAs(takingAnimationSettings.GetParams());
                
                // Mark the item's slot as taken
                shoppingListItemUIDictionary[shelfItem.Item].color = itemTakenColor;
                
                takenShelfItems.Add(shelfItem.Item);
                shelfItemTakenUnityEvent?.Invoke(shelfItem);
                
                if (takenShelfItems.Count >= shoppingListItemUIDictionary.Count)
                {
                    allShelfItemsTakenUnityEvent?.Invoke();
                    mainCamera.transform.DOKill();
                    Debug.Log("All items taken!");
                }
                
                Debug.Log($"Item taken: {shelfItem.name}");
            }
        }
    }
}