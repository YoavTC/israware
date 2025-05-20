using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using External_Packages.Extensions;
using UnityEngine;

namespace _Game_Assets.Microgames.shoppingList
{
    public class ShelfItemsManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ShoppingList shoppingList;
        
        [Header("Settings")]
        [SerializeField, DisableInPlayMode] private int shoppingListSize;
        [SerializeField, DisableInPlayMode] private int maxDecoys;
        
        [Header("Shelf Item Prefabs")]
        [SerializeField] private ShelfItem shelfItemPrefab;
        [SerializeField] private List<ShelfItemScriptableObject> shelfItems;
         
        [Header("Shelf Item Settings")]
        [SerializeField] private float shelfItemXMultiplier;
        [SerializeField] private float shelfItemZOffset;
        [SerializeField] private Vector2[] shelfItemHeights;
        
        [SerializeField, HideInEditMode] private int itemsSpawned;
        [SerializeField, HideInEditMode] private int lastSpawnedShelf;

        public void Start()
        {
            ShelfItemScriptableObject[] items = shelfItems.Shuffle().Take(shoppingListSize).ToArray();
            ShelfItemScriptableObject[] decoys = shelfItems.Except(items).ToArray();

            items.Shuffle();
            decoys.Shuffle();
            
            shoppingList.FillShoppingList(items);
            StockShelves(items, decoys);
        }

        private void StockShelves(ShelfItemScriptableObject[] items, ShelfItemScriptableObject[] decoys)
        {
            // Duplicate the decoys to fill the shelves, but limit to the max
            decoys = decoys.Concat(decoys) // Duplicate
                .ToArray().Shuffle() // Shuffle again
                .Take(maxDecoys).ToArray(); // Limit to max decoys

            // Duplicate the items to give the player more chances to find them
            items = items.Concat(items).ToArray();
            
            items.Concat(decoys)
                .ToArray()
                .Shuffle()
                .ForEach(SpawnItem);
        }

        private void SpawnItem(ShelfItemScriptableObject item)
        {
            ShelfItem shelfItem = Instantiate(shelfItemPrefab, transform);
            shelfItem.Init(item);
            
            itemsSpawned++;
            
            int randomShelf = UnityEngine.Random.Range(0, shelfItemHeights.Length);
            if (randomShelf == lastSpawnedShelf) 
            {
                randomShelf = (randomShelf + 1) % shelfItemHeights.Length;
            }
            lastSpawnedShelf = randomShelf;

            shelfItem.transform.position = new Vector3(itemsSpawned * shelfItemXMultiplier, shelfItemHeights[randomShelf].y, shelfItemZOffset);
        }
    }
}
