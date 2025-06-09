using System;
using UnityEngine;

namespace _Game_Assets.Microgames.geoGuessr
{
    public class MusicPitchShifter : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private AnimationCurve pitchCurve;
        [SerializeField] private float pitchCurveStepMultiplier;
        
        void Start()
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        private void Update()
        {
            audioSource.pitch = 1 + pitchCurve.Evaluate(Time.timeSinceLevelLoad * pitchCurveStepMultiplier);
        }
    }
}
