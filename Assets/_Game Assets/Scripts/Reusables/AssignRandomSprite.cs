using UnityEngine;

namespace _Game_Assets.Scripts.Reusables
{
    public class AssignRandomSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] sprites;
        
        private void Awake()
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            Destroy(this);
        }
    }
}
