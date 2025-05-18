using System;
using System.Collections.Generic;
using EditorAttributes;
using External_Packages.Extensions;
using UnityEngine;

namespace _Game_Assets.Microgames.shoppingList
{
    public class ShelfItemsManager : MonoBehaviour
    {
        [SerializeField] private ShelfItem shelfItemPrefab;
        [SerializeField] private List<ShelfItemScriptableObject> shelfItems;
        [Space]
        [SerializeField] private Vector2[] shelfItemHeights;
        
        [SerializeField] private Notebook notebook;

        [Button]
        public void SpawnItem()
        {
            ShelfItem shelfItem = Instantiate(shelfItemPrefab, transform);
            shelfItem.Init(shelfItems.Random());
        }

        public void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                ShelfItemScriptableObject item = shelfItems.Random();
                shelfItems.Remove(item);
                
                notebook.AddItem(item);
            }
        }
    }
}
