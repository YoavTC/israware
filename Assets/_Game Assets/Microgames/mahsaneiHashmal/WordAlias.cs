using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.mahsaneiHashmal
{
    public class WordAlias : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private void Awake()
        {
            ToggleBackground(false);

            elapsedTime = 0f;
            isSliding = true;
            randomDirection = Random.insideUnitCircle.normalized;
        }
        
        #region Hover Effect
        [SerializeField] private GameObject background;
        
        public void OnPointerEnter(PointerEventData eventData) => ToggleBackground(true);
        public void OnPointerExit(PointerEventData eventData) => ToggleBackground(false);

        private void ToggleBackground(bool state)
        {
            background.SetActive(state);
        }
        #endregion
        
        #region Spawn Animation
        [SerializeField] private Vector2 velocityMultiplierRange;
        [SerializeField] private AnimationCurve velocityCurve;

        private float elapsedTime;
        private Vector3 randomDirection;
        private bool isSliding;

        // Called from the WordManager when the word alias is grabbed
        public void OnWordGrabbed() => isSliding = false;
        
        // Animate spawning effect
        private void Update()
        {
            if (isSliding)
            {
                elapsedTime += Time.deltaTime;
                
                float velocity = velocityCurve.Evaluate(elapsedTime);
                
                velocity *= Screen.width; // Screen size-responsive
                velocity *= Time.deltaTime; // Frame rate-independent
                velocity *= Random.Range(velocityMultiplierRange.x, velocityMultiplierRange.y); // Variation
                
                transform.position += randomDirection * velocity;
            }
        }
        #endregion
    }
}
