using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace _Game_Assets.Scripts
{
    public class StatusScreen : MonoBehaviour
    {
        [SerializeField] private StateMachine stateMachine;
        
        [SerializeField] private GameObject screenParent;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RenderTexture renderTexture;
        
        [SerializeField] private VideoClip positiveVideoClip;
        [SerializeField] private VideoClip negativeVideoClip;

        public void ShowStatus(bool won)
        {
            videoPlayer.clip = won ? positiveVideoClip : negativeVideoClip;
            
            screenParent.SetActive(true);
            videoPlayer.Play();
            videoPlayer.loopPointReached += source => stateMachine.ChangeState(State.GAME);
        }

        public void HideStatus()
        {
            screenParent.SetActive(false);
            videoPlayer.clip = null;
            renderTexture.Release();
        }
    }
}