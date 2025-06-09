using External_Packages.Extensions;
using UnityEngine;

namespace _Game_Assets.Scripts.Reusables
{
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] audioClips;
        
        private void Start()
        {
            if (audioSource == null) audioSource = gameObject.GetOrAdd<AudioSource>();

            audioSource.clip = audioClips.Random();
            audioSource.Play();
        }
    }
}
