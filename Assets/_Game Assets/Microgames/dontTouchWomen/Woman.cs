using DG.Tweening;
using External_Packages.Extra_Components;
using UnityEngine;
using UnityUtils;

namespace _Game_Assets.Microgames.dontTouchWomen
{
    public class Woman : MonoBehaviour
    {
        private Transform hasidicTransform;
        private float speed;
        private float touchDistance;

        private bool allowMove;
        
        public void Init(Transform hasidicTransform, float speed, float touchDistance)
        {
            this.hasidicTransform = hasidicTransform;
            this.speed = speed;
            this.touchDistance = touchDistance;

            allowMove = true;
            
            GetComponentsInChildren<TweenScaleEffect>().ForEach(eye => eye.DoEffect());
        }

        private void Update()
        {
            if (allowMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, hasidicTransform.position, speed * Time.deltaTime);
                if (Vector2.Distance(transform.position, hasidicTransform.position) < touchDistance)
                {
                    hasidicTransform.GetComponentInParent<HasidicController>().TouchHasidic();
                    StopWoman();
                }
            }
        }

        public void StopWoman()
        {
            allowMove = false;
            GetComponentsInChildren<TweenScaleEffect>().ForEach(eye => eye.transform.DOKill());
        }
    }
}