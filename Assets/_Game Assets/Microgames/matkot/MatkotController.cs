using System;
using System.Collections;
using DG.Tweening;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.matkot
{
    public class MatkotController : CooldownAction
    {
        [Header("Components")]
        [SerializeField] private Transform paddle;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D paddleCollider;
        [SerializeField] private Camera mainCamera;

        [Header("Cooldown")]
        [SerializeField] private float useCooldown;
        [SerializeField] private float useDuration;
        [SerializeField] private bool canUse;
        [SerializeField] private Sprite defaultSprite, useSprite;
        
        [Header("Deflection")]
        [SerializeField] private Vector2[] deflectedBallsTargetPoints;
        [SerializeField] private float deflectedBallsSpeed;
        [SerializeField] private UnityEvent paddleHitBallUnityEvent;
        
        [Header("Rotation")]
        [SerializeField] private Vector2 rotationAnchorPoint;
        [SerializeField] private Vector2 rotationOffset;

        private const float Z_POSITION = 10f;

        private void Start()
        {
            ActionEnabled = true;
            ActionCooldown = useCooldown;

            canUse = true;
        }

        protected override void Update()
        {
            base.Update();
            
            MovePaddle();
            RotatePaddle();

            if (Input.GetMouseButtonDown(0) && canUse)
            {
                StartCoroutine(UsePaddle());
            }
        }

        protected override void ExecuteAction()
        {
            canUse = true;
            ActionEnabled = false;
        }

        private IEnumerator UsePaddle()
        {
            spriteRenderer.sprite = useSprite;
            paddleCollider.enabled = true;
            
            yield return new WaitForSeconds(useDuration);

            spriteRenderer.sprite = defaultSprite;
            paddleCollider.enabled = false;
            
            canUse = false;
            ActionEnabled = true;
        }

        private void RotatePaddle()
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Ensure the calculation happens in 2D space

            Vector2 direction = (Vector2)mousePosition - rotationAnchorPoint;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            paddle.rotation = Quaternion.Euler(0, 0, angle + rotationOffset.x);
        }

        private void MovePaddle()
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = Z_POSITION;
            
            paddle.position = mousePosition;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("NotPlayer"))
            {
                // Get ball & remove collider component
                Transform ball = other.transform;
                Destroy(other);

                // Stop it's Tweens
                ball.DOKill(false);
                ball.DOComplete(false);
                
                // Start new going out Tweens
                ball.DOMove(deflectedBallsTargetPoints[Random.Range(0, deflectedBallsTargetPoints.Length)], deflectedBallsSpeed).OnComplete(() => Destroy(ball.gameObject));
                ball.DOScale(0f, deflectedBallsSpeed * 0.65f).SetEase(Ease.Linear);

                // Modify it's Trail Renderer
                TrailRenderer ballTrailRenderer = ball.GetComponent<TrailRenderer>();
                ballTrailRenderer.time *= 1.5f;
                ballTrailRenderer.DOResize(0f, 0f, deflectedBallsSpeed);
                ballTrailRenderer.endColor = new Color(1f, 1f, 1f, 0.5f);
                ballTrailRenderer.startColor = new Color(1f, 1f, 1f, 0.5f);
                
                paddleHitBallUnityEvent?.Invoke();
            }
        }
    }
}
