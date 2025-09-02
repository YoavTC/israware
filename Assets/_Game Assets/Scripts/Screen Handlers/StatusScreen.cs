using System;
using _Game_Assets.Scripts.Definitions;
using External_Packages.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

namespace _Game_Assets.Scripts.Screen_Handlers
{
    public class StatusScreen : BaseScreen
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RenderTexture renderTexture;

        [SerializeField] private VideoClip[] positiveVideoClips;
        [SerializeField] private VideoClip[] negativeVideoClips;

        private int lastPositiveVideoHashcode;
        private int lastNegativeVideoHashcode;


        private bool finishedPlaying = false;

        
        public override void Show(bool won)
        {
            finishedPlaying = false;

            videoPlayer.clip = GetVideoClip(won);

            screenParent.SetActive(true);
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        private void OnVideoFinished(VideoPlayer source)
        {
            if (finishedPlaying) return;

            finishedPlaying = true;
            Debug.Log($"Video {source.clip.name} finished playing");
            stateMachine.ChangeState(State.GAME);
        }

        public override void Hide()
        {
            screenParent.SetActive(false);
            videoPlayer.clip = null;
            renderTexture.Release();
        }

        private VideoClip GetVideoClip(bool positive)
        {
            VideoClip clip = positive ? positiveVideoClips.Random() : negativeVideoClips.Random();

            if (clip.GetHashCode() == lastPositiveVideoHashcode || clip.GetHashCode() == lastNegativeVideoHashcode)
            {
                clip = GetVideoClip(positive);
            }

            if (positive) lastPositiveVideoHashcode = clip.GetHashCode();
            else lastNegativeVideoHashcode = clip.GetHashCode();
            
            return clip;
        }
    }
}