using External_Packages.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace _Game_Assets.Microgames.shoppingList
{
    public class ShoppingList : MonoBehaviour
    {
        public void FillShoppingList(ShelfItemScriptableObject[] items)
        {
            foreach (ShelfItemScriptableObject item in items)
            {
                var itemSlot = new GameObject("ItemSlot");
                itemSlot.transform.SetParent(transform, false);

                itemSlot.GetOrAdd<RectTransform>();
                itemSlot.GetOrAdd<CanvasRenderer>();
                itemSlot.GetOrAdd<Image>().sprite = item.itemNameSprite;
                
                Debug.Log($"Added {item.name} to shopping list");
            }
        }
    }
}