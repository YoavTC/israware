using External_Packages.Extensions;
using UnityEngine;

namespace _Game_Assets.Microgames.shoppingList
{
    public class ShelfItem : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider boxCollider;
        
        [Header("Shadow Settings")]
        [SerializeField] private Vector3 shadowOffset;
        [SerializeField] private Color shadowColor;
        
        private ShelfItemScriptableObject item;
        public ShelfItemScriptableObject Item => item;
        
        public void Init(ShelfItemScriptableObject item)
        {
            this.item = item;
            name = item.name;
            
            // Assign the item sprite
            // If the item sprite is an array, randomly choose one
            spriteRenderer.sprite = item.itemSprite.Random();
            
            // Spawn a shadow
            SpriteRenderer shadow = Instantiate(spriteRenderer, transform);
            shadow.transform.localPosition = shadowOffset;
            shadow.transform.localScale = Vector3.one;
            shadow.sprite = spriteRenderer.sprite;
            shadow.color = shadowColor;
            
            // Remove the shadow's collider
            Destroy(shadow.GetComponent<BoxCollider>());
        }

        public void OnItemTaken()
        {
            boxCollider.enabled = false;
        }
    }
}