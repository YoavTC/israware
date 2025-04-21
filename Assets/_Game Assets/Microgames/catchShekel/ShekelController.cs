using UnityEngine;

namespace _Game_Assets.Microgames.catchShekel
{
    public class ShekelController : MonoBehaviour
    {
        [SerializeField] private Transform shekelTransform;
        [Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;
        [Space]
        [SerializeField] private Vector2 minMaxXPosition;
        
        public Transform ShekelTransform => shekelTransform;

        private bool moveRight;
        private bool rotateRight;
        
        private bool allowMove = true;
        
        void Update()
        {
            if (!allowMove) return;
            
            shekelTransform.position += (moveRight ? Vector3.right : Vector3.left) * (moveSpeed * Time.deltaTime);
            shekelTransform.Rotate(moveRight ? Vector3.back : Vector3.forward, rotateSpeed * Time.deltaTime);
            
            
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
