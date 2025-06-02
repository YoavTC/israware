using System;
using System.Collections;
using System.Linq;
using _Game_Assets.Scripts.Definitions;
using DG.Tweening;
using External_Packages.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace _Game_Assets.Scripts.ScreenHandlers 
{
    public class StatusScreenHandler : ScreenHandlerBase
    {
        [SerializeField] private TMP_Text levelDisplay;
        [SerializeField] private VideoPlayer positiveVideoPlayer;
        [SerializeField] private VideoPlayer negativeVideoPlayer;
        
        [SerializeField] private VideoClip[] positiveVideos;
        [SerializeField] private VideoClip[] negativeVideos;

        [SerializeField] private RenderTexture positiveRenderTexture;
        [SerializeField] private RenderTexture negativeRenderTexture;
        
        [SerializeField] private RawImage videoRenderer;
        
        public override IEnumerator Show(float duration, bool wonLastMicrogame, int newHealth, int newScore)
        {
            screenParent.SetActive(true);

            if (wonLastMicrogame)
            {
                videoRenderer.texture = positiveRenderTexture;
                positiveVideoPlayer.Play();
            } 
            else
            {
                videoRenderer.texture = negativeRenderTexture;
                negativeVideoPlayer.Play();
            }

            if (wonLastMicrogame)
            {
                yield return new WaitForSeconds(1.5f);
                
                levelDisplay.text = newScore.ToString();
                levelDisplay.transform.DOPunchScale(Vector3.one, 0.2f);
            }
            
            if (duration > 0)
            {
                yield return new WaitForSeconds(duration);
                screenParent.SetActive(false);
            }

            yield return new WaitForSeconds((float) positiveVideoPlayer.length);
            LoadNextVideoClips();
        }
        
        private void LoadNextVideoClips()
        {
            positiveVideoPlayer.clip = positiveVideos.Random();
            negativeVideoPlayer.clip = negativeVideos.Random();
            
            positiveVideoPlayer.Prepare();
            negativeVideoPlayer.Prepare();
        }
    }
}