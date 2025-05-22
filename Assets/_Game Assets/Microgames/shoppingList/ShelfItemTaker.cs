using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.shoppingList
{
    public class ShelfItemTaker : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Camera mainCamera;

        [HideInEditMode, SerializeField] private bool allowTaking;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<ShelfItem> shelfItemTakenUnityEvent;

        private void Start()
        {
            allowTaking = true;
        }
        
        public void ToggleAllowTaking(bool state) => allowTaking = state;

        void Update()
        {
            if (!allowTaking) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                RaycastShelfItem();
            }
        }

        private void RaycastShelfItem()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ShelfItem shelfItem = hit.collider.GetComponent<ShelfItem>();
                if (shelfItem != null)
                {
                    shelfItemTakenUnityEvent?.Invoke(shelfItem);
                    shelfItem.OnItemTaken();
                    
                    Debug.Log($"Item clicked: {shelfItem.name}");
                }
            }
        }
    }
}
