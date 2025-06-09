using System;
using DG.Tweening;
using External_Packages.Extensions;
using External_Packages.Extra_Components;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.dontTouchWomen
{
    public class HasidicController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform hasidicTransform;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource cryAudioSource;
        
        [SerializeField] private float zPosition;
        [SerializeField] private float movementSpeed;

        private Vector3 mousePosition;
        private bool allowMove;

        [Space] 
        [SerializeField] private Transform[] eyebrows;
        [SerializeField] private Vector2 bobDirection;
        [SerializeField] private float bobDuration;
        
        [Header("Audio")]
        [SerializeField] private AudioClip[] ambientAudioClips;
        [SerializeField] private AudioClip[] deathAudioClips;
        [SerializeField] private AudioClip[] cryAudioClips;
        [Space]
        [SerializeField] private Vector2 intervalRange;
        private float nextInterval;
        private float audioTimer;
        
        [SerializeField] private UnityEvent hasidicTouchedUnityEvent;

        private void Start()
        {
            allowMove = true;
            foreach (Transform eyebrow in eyebrows)
            {
                eyebrow.DOLocalMove(eyebrow.position + (Vector3)bobDirection, bobDuration)
                    .SetLoops(-1, LoopType.Yoyo);
                
                bobDirection.x *= -1;
            }
        }

        void Update()
        {
            if (allowMove)
            {
                // Get the mouse position in world space
                mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = zPosition;
                
                hasidicTransform.position = Vector3.Lerp(hasidicTransform.position, mousePosition, Time.deltaTime * movementSpeed);
                
                lineRenderer.SetPosition(0, hasidicTransform.position);
                lineRenderer.SetPosition(1, mousePosition);
                
                audioTimer+= Time.deltaTime;
                if (audioTimer >= nextInterval)
                {
                    audioTimer = 0f;
                    nextInterval = UnityEngine.Random.Range(intervalRange.x, intervalRange.y);
                    PlayAmbientAudio();
                }
            }
        }

        private void PlayAmbientAudio()
        {
            audioSource.clip = ambientAudioClips.Random();
            audioSource.Play();
        }

        private void PlayDeathAudio()
        {
            audioSource.Stop();
            audioSource.clip = deathAudioClips.Random();
            audioSource.Play();
        }
        
        private void PlayCryAudio()
        {
            cryAudioSource.clip = cryAudioClips.Random();
            cryAudioSource.Play();
        }

        public void TouchHasidic()
        {
            allowMove = false;
            hasidicTouchedUnityEvent?.Invoke();
            
            PlayDeathAudio();
            PlayCryAudio();
        }
    }
}
