using UnityEngine;
using UnityEngine.UI;

namespace _Game_Assets.Microgames.shoppingList
{
    public class Notebook : MonoBehaviour
    {
        [SerializeField] private Image[] itemSlots;
        private int itemsCount;

        private void Start()
        {
            itemsCount = 0;
            foreach (Image itemSlot in itemSlots)
            {
                itemSlot.sprite = null;
            }
        }

        public void AddItem(ShelfItemScriptableObject item)
        {
            if (itemsCount >= itemSlots.Length) return;
            
            itemSlots[itemsCount].sprite = item.itemSprite;
            itemsCount++;
        }
    }
}