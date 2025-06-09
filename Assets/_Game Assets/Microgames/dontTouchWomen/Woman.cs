using DG.Tweening;
using External_Packages.Extensions;
using External_Packages.Extra_Components;
using UnityEngine;

namespace _Game_Assets.Microgames.dontTouchWomen
{
    public class Woman : MonoBehaviour
    {
        private Transform hasidicTransform;
        private float speed;
        private float touchDistance;

        private bool allowMove;
        
        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private Vector2 audioPitchRange = new Vector2(0.8f, 1.2f);
        [Space]
        [SerializeField] private float catcallInterval;
        [SerializeField] private float catcallVolume;
        [SerializeField, Range(0f, 1f)] private float catcallProbability = 0.1f;
        [SerializeField] private Vector2 catcallDelayRange = new Vector2(0.5f, 1.5f);
        
        private float catcallTimer;
        
        public void Init(Transform hasidicTransform, float speed, float touchDistance)
        {
            this.hasidicTransform = hasidicTransform;
            this.speed = speed;
            this.touchDistance = touchDistance;

            allowMove = true;
            
            GetComponentsInChildren<TweenScaleEffect>().ForEach(eye => eye.DoEffect());
            
            Catcall();
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

                catcallTimer += Time.deltaTime;
                if (catcallTimer >= catcallInterval)
                {
                    catcallTimer = 0f;
                    Catcall();
                }
            }
        }

        private void Catcall()
        {
            if (catcallProbability < Random.value)
                return;
            
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = catcallVolume;
            audioSource.clip = audioClips.Random();
            audioSource.pitch = Random.Range(audioPitchRange.x, audioPitchRange.y);
            
            float delay = Random.Range(catcallDelayRange.x, catcallDelayRange.y);
            audioSource.PlayDelayed(delay);
            Destroy(audioSource, delay + audioSource.clip.length);
        }

        public void StopWoman()
        {
            allowMove = false;
            GetComponentsInChildren<TweenScaleEffect>().ForEach(eye => eye.transform.DOKill());
        }
    }
}