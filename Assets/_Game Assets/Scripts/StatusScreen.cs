using UnityEngine;
using UnityEngine.Video;

namespace _Game_Assets.Scripts
{
    public class StatusScreen : BaseScreen
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RenderTexture renderTexture;
        
        [SerializeField] private VideoClip positiveVideoClip;
        [SerializeField] private VideoClip negativeVideoClip;
        
        public override void Show(bool won)
        {
            videoPlayer.clip = won ? positiveVideoClip : negativeVideoClip;
            
            screenParent.SetActive(true);
            videoPlayer.Play();
            videoPlayer.loopPointReached += source => stateMachine.ChangeState(State.GAME);
        }

        public override void Hide()
        {
            screenParent.SetActive(false);
            videoPlayer.clip = null;
            renderTexture.Release();
        }
    }
}