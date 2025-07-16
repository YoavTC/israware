using System;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Microgames.woltFrogger
{
    public class WolterController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        [SerializeField] private SpriteRenderer[] spriteRenderers;
        
        private Vector2 movement;
        private bool allowMovement = true;
        
        void Update()
        {
            if (!allowMovement) return;
            
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
            transform.position += (Vector3) movement.normalized * (Time.deltaTime * moveSpeed);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Respawn")) OnHit();
        }

        [Button]
        public void OnHit()
        {
            allowMovement = false;

            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.DOFade(0f, 0.5f);
            }
            
            transform.DOScale(1.5f, 1f).SetEase(Ease.OutExpo);
            transform.DOPunchRotation(Vector3.back * 50, 1f).SetEase(Ease.OutExpo);
            transform.DOMoveX(transform.position.x + 0.5f, 0.5f).SetEase(Ease.OutExpo);
        }
    }
}
