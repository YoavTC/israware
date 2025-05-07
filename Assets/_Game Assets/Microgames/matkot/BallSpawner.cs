using _Game_Assets.Scripts;
using DG.Tweening;
using External_Packages.MonoBehaviour_Extensions;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.matkot
{
    public class BallSpawner : CooldownAction
    {
        [Header("Spawner")] 
        [SerializeField] private Transform spawnerParent;
        [SerializeField] private int spawnAmount;
        [SerializeField] private float spawnCooldown;
        [SerializeField] private float spawnCooldownReducePerStep;
        private int ballsSpawned;

        [Header("Points")]
        [SerializeField] private Vector2[] spawnOriginPoint;
        [SerializeField] private Vector2[] targetPoints;

        [Header("Projectile")] 
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Vector2 minMaxBallPathHeight;
        [SerializeField] private Vector2 minMaxBallTravelDuration;
        private MMF_SpriteRenderer flickerFeedback;

        [Header("UI")]
        [SerializeField] private TMP_Text remainingBallsCounterDisplay;

        [Header("Events")]
        [SerializeField] private UnityEvent missedBallUnityEvent;
        
        private void Start()
        {
            ActionEnabled = true;
            ActionCooldown = spawnCooldown;
            ballsSpawned = 0;

            flickerFeedback = spawnerParent.GetComponent<MMF_Player>().GetFeedbackOfType<MMF_SpriteRenderer>();
            UpdateUICounter();
        }

        protected override void ExecuteAction()
        {
            ShootBall();
            ActionCooldown -= spawnCooldownReducePerStep;
            
            ballsSpawned++;
            if (ballsSpawned >= spawnAmount)
            {
                ActionEnabled = false;
            }
        }

        private void ShootBall()
        {
            var ball = Instantiate(ballPrefab, spawnOriginPoint[Random.Range(0, spawnOriginPoint.Length)], Quaternion.identity, spawnerParent).transform;

            flickerFeedback.BoundSpriteRenderer = ball.GetComponent<SpriteRenderer>();
            
            ball.GetComponent<SpriteRenderer>().color = External_Packages.Random.RandomBool() ? Color.black : Color.red;

            float lifetime = GetRandomFromVector2(minMaxBallTravelDuration);
            ball.DOMove(targetPoints[Random.Range(0, targetPoints.Length)], lifetime)
                .SetEase(Ease.Linear)
                .OnComplete(() => DestroyBall(ball));
            
            ball.DORotate(new Vector3(0, 0, 360), lifetime, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        private void DestroyBall(Transform ball)
        {
            ball.DOKill(true);
            Destroy(ball.gameObject);
            
            Debug.Log("Ball reached end");
            missedBallUnityEvent?.Invoke();
        }

        public void UpdateUICounter()
        {
            remainingBallsCounterDisplay.text = (spawnAmount - ballsSpawned).ToString("D2");
        }

        private float GetRandomFromVector2(Vector2 vector)
        {
            return Random.Range(vector.x, vector.y);
        }
    }
}
