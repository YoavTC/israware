using System;
using UnityEngine;

namespace _Game_Assets.Scripts.Definitions
{
    [Serializable]
    public class AudioClipSettings
    {
        public AudioClip audioClip;
        public float volume;
        public float pitch;

        public AudioClipSettings(AudioClip audioClip, float volume = 1f, float pitch = 1f)
        {
            this.audioClip = audioClip;
            this.volume = volume;
            this.pitch = pitch;
        }
    }
}