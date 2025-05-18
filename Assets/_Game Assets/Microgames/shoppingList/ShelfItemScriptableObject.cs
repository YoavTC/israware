using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Microgames.shoppingList
{
    [CreateAssetMenu(fileName = "ShelfItemScriptableObject", menuName = "Scriptable Objects/ShelfItemScriptableObject")]
    public class ShelfItemScriptableObject : ScriptableObject
    {
        [SerializeField, HorizontalGroup(nameof(itemSprite), nameof(itemNameSprite))] private Void spritesHolder;
        
        [AssetPreview, HideInInspector] public Sprite itemSprite;
        [AssetPreview, HideInInspector] public Sprite itemNameSprite;
    }
}
