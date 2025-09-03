using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using _Game_Assets.Scripts.Reusables;

namespace _Game_Assets.Microgames.benGvirKipa
{
    public class BenGvirController : MonoBehaviour
    {
        [SerializeField] private Transform kipaTransform;
        [SerializeField] private Transform benGvirTransform;


        [SerializeField] private float benGvirMoveCooldown;
        private float benGvirMoveTimer;

        [SerializeField] private Vector2 kipaPitchMinMax;
        [SerializeField] private Vector2 kipaMinMaxXPositions;
        [SerializeField] private TweenSettings kipaFallTweenSettings;
        [SerializeField] private float kipaGoDownDelay;
        private float kipaGoDownTimer;
        private bool moveKipa;

        [SerializeField] private UnityEvent BenGvirMoveUnityEvent;
        [SerializeField] private UnityEvent<float> KipaGoDownUnityEvent;
        [SerializeField] private UnityEvent SuccessUnityEvent;
        [SerializeField] private UnityEvent FailUnityEvent;

        void Start()
        {
            moveKipa = true;
            int randomX = Mathf.CeilToInt(Random.Range(kipaMinMaxXPositions.x, kipaMinMaxXPositions.y));
            kipaTransform.position = new Vector3(randomX, kipaTransform.position.y, kipaTransform.position.z);
        }


        void Update()
        {
            if (!moveKipa) return;

            HandleInput();

            kipaGoDownTimer += Time.deltaTime;
            if (kipaGoDownTimer >= kipaGoDownDelay)
            {
                kipaGoDownTimer = 0f;
                kipaTransform.position += Vector3.down;

                if (kipaTransform.position.y <= -3)
                {
                    moveKipa = false;
                    CheckCaught();
                }
                else
                {
                    float normalizedKipaYPosition = Mathf.InverseLerp(4, -3, kipaTransform.position.y);
                    normalizedKipaYPosition = Mathf.Lerp(kipaPitchMinMax.x, kipaPitchMinMax.y, normalizedKipaYPosition);
                    KipaGoDownUnityEvent?.Invoke(normalizedKipaYPosition);
                }

            }
        }

        private void CheckCaught()
        {
            if (kipaTransform.position.x == benGvirTransform.position.x)
            {
                SuccessUnityEvent?.Invoke();
            }
            else
            {
                kipaTransform.DOMoveY(-6f, kipaFallTweenSettings.duration)
                    .SetAs(kipaFallTweenSettings.GetParams());

                FailUnityEvent?.Invoke();
            }
        }

        private void HandleInput()
        {
            benGvirMoveTimer += Time.deltaTime;

            if (benGvirMoveTimer > benGvirMoveCooldown)
            {
                if (Input.GetKey(KeyCode.LeftArrow)) MoveBenGvir(-1);
                else if (Input.GetKey(KeyCode.RightArrow)) MoveBenGvir(1);
            }
        }

        private void MoveBenGvir(int dir)
        {
            benGvirMoveTimer = 0f;
            BenGvirMoveUnityEvent?.Invoke();

            int clampedX = Mathf.Clamp(Mathf.CeilToInt(benGvirTransform.position.x) + dir, -5, 5);
            benGvirTransform.position = new Vector3(clampedX, benGvirTransform.position.y, benGvirTransform.position.z);
        }
    }
}
