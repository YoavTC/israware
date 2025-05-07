using System;
using System.Linq;
using EditorAttributes;
using External_Packages.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.staticToBenEl
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator playerAnimator;
        [Space]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform targetTransform;
        [Space]
        [SerializeField] private SpriteRenderer playerSpriteRenderer;
        [SerializeField] private SpriteRenderer targetSpriteRenderer;
        [Space]
        [SerializeField] private Sprite[] playerSprites;

        [Header("Animations")] 
        [SerializeField, AnimatorParamDropdown(nameof(playerAnimator))] private string animatorWalkKey;
        
        [Header("Settings")]
        [SerializeField] private float targetRadius;
        [SerializeField] private float playerSpeed;
        
        [Header("Events")]
        [SerializeField] private UnityEvent playerReachedTargetUnityEvent;

        private Vector2 movement;
        
        private void Start()
        {
            SelectRandomPlayerSprite();
        }

        private void SelectRandomPlayerSprite()
        {
            Sprite randomSprite = playerSprites.Random();
            Sprite otherSprite = playerSprites.Where(sprite => sprite != randomSprite).ToArray()[0];
            
            playerSpriteRenderer.sprite = randomSprite;
            targetSpriteRenderer.sprite = otherSprite;
        }

        void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
            playerAnimator.SetBool(animatorWalkKey, movement != Vector2.zero);

            if (movement.x != 0)
            {
                playerSpriteRenderer.flipX = movement.x < 0;
            }
            
            if (Vector2.Distance(playerTransform.position, targetTransform.position) < targetRadius)
            {
                playerAnimator.SetBool(animatorWalkKey, false);
                playerReachedTargetUnityEvent?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            rb.position += movement.normalized * (playerSpeed * Time.fixedDeltaTime);
        }
    }
}
