using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Scripts.Reusables
{
    public class ClickCallbackEvent : MonoBehaviour
    {
        [SerializeField, Required] private Collider2D hitbox;
        private Camera mainCamera;

        [SerializeField] private bool destroyAfterClick;
        [SerializeField] private UnityEvent clickUnityEvent;
        
        private void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                if (hitbox.OverlapPoint(mousePosition))
                {
                    clickUnityEvent?.Invoke();
                    
                    if (destroyAfterClick) Destroy(this);
                }
            }
        }
    }
}
