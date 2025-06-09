using _Game_Assets.Scripts.Definitions;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class SFXManager : MonoBehaviour, IMicrogameCallbacksListener
    {
        public void PlaySFX(AudioClip audioClip)
        {
            PlaySFX(new AudioClipSettings(audioClip));
        }
    
        public void PlaySFX(AudioClipSettings audioClipSettings)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClipSettings.audioClip;
            audioSource.volume = audioClipSettings.volume;
            audioSource.pitch = audioClipSettings.pitch;
        
            audioSource.Play();
            Destroy(audioSource, audioClipSettings.audioClip.length);
        }
    
        public void OnMicrogameLoaded(MicrogameScriptableObject microgame) => StopAllSounds();
        public void OnMicrogameFinished(bool result) => StopAllSounds();
    
        public void StopAllSounds()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                Destroy(audioSource);
            }
        }
    }
}