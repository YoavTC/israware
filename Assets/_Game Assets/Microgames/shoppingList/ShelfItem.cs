using UnityEngine;

namespace _Game_Assets.Microgames.shoppingList
{
    public class ShelfItem : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Shadow Settings")]
        [SerializeField] private Vector3 shadowOffset;
        [SerializeField] private Color shadowColor;
        
        private ShelfItemScriptableObject item;
        
        public void Init(ShelfItemScriptableObject item)
        {
            this.item = item;
            spriteRenderer.sprite = item.itemSprite;
            
            // Spawn a shadow
            SpriteRenderer shadow = Instantiate(spriteRenderer, transform);
            shadow.transform.localPosition = shadowOffset;
            shadow.transform.localScale = Vector3.one;
            shadow.sprite = item.itemSprite;
            shadow.color = shadowColor;
        }
    }
}