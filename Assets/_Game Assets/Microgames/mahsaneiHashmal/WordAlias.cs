using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.mahsaneiHashmal
{
    public class WordAlias : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject background;
        
        private void Awake()
        {
            ToggleBackground(false);
            
            isSliding = true;
            direction = Random.insideUnitCircle.normalized;
        }

        public void OnPointerEnter(PointerEventData eventData) => ToggleBackground(true);
        public void OnPointerExit(PointerEventData eventData) => ToggleBackground(false);

        private void ToggleBackground(bool state)
        {
            background.SetActive(state);
        }

        [SerializeField] private float velocityMultiplier;
        [SerializeField] private AnimationCurve velocityCurve;
        private Vector3 direction;
        private bool isSliding;

        public void OnWordGrabbed() => isSliding = false;
        
        private void Update()
        {
            if (isSliding)
            {
                float velocity = velocityCurve.Evaluate(Time.time) * Screen.width;
                transform.position += direction * (velocityMultiplier * Time.deltaTime * velocity);
            }
        }

    }
}
