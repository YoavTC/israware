using UnityEngine;

namespace _Game_Assets.Microgames.catchShekel
{
    public class ShekelController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform shekelTransform;
        public Transform ShekelTransform => shekelTransform;
        
        [Header("Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;
        [Space]
        [SerializeField] private Vector2 minMaxXPosition;
        
        private bool moveRight;
        
        void Update()
        {
            // Move and rotate the shekel
            shekelTransform.position += (moveRight ? Vector3.right : Vector3.left) * (moveSpeed * Time.deltaTime);
            shekelTransform.Rotate(moveRight ? Vector3.back : Vector3.forward, rotateSpeed * Time.deltaTime);
            
            // Bounds check
            if (shekelTransform.position.x > minMaxXPosition.y)
            {
                moveRight = false;
            }
            else if (shekelTransform.position.x < minMaxXPosition.x)
            {
                moveRight = true;
            }
        }
    }
}
