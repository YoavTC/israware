using UnityEngine;

namespace _Game_Assets.Microgames.splitRedSea
{
    public class HandRotator : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform target;
        [SerializeField] private RectTransform hand;
        
        void Update()
        {
            Vector2 targetPos = mainCamera.WorldToScreenPoint(target.position);
            hand.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetPos.y - hand.position.y, targetPos.x - hand.position.x) * Mathf.Rad2Deg);
        }
    }
}
