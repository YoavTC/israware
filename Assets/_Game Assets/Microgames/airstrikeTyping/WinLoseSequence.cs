using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace _Game_Assets.Microgames.airstrikeTyping
{
    public class WinLoseSequence : MonoBehaviour
    {
        [SerializeField] private MMF_Player feedbacksPlayer;
        
        [SerializeField] private RectTransform[] loseScreens;
        [SerializeField] private RectTransform[] winScreens;

        [SerializeField] private float sequenceDelay;
        [SerializeField] private float explosionDelay;
        [SerializeField] private float explosionDuration;
        [SerializeField] private float explosionScale;

        private Image explodedScreen;
        private Image explosion;
        
        public void ShowScreen(bool win)
        {
            RectTransform screen = win ? winScreens[Random.Range(0, winScreens.Length)] : loseScreens[Random.Range(0, loseScreens.Length)];
            
            explodedScreen = screen.GetChild(0).GetComponent<Image>();
            explosion = screen.GetChild(1).GetComponent<Image>();
            
            StartCoroutine(Sequence(screen.GetComponent<Image>()));
        }

        private IEnumerator Sequence(Image screen)
        {
            yield return new WaitForSeconds(sequenceDelay);
            
            screen.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(explosionDelay);
            
            explosion.gameObject.SetActive(true);
            screen.sprite = explodedScreen.sprite;
            
            explosion.transform.DOScale(Vector3.one * explosionScale, explosionDuration);
            explosion.DOFade(0f, explosionDuration);
            
            feedbacksPlayer.PlayFeedbacks();
        }
    }
}
