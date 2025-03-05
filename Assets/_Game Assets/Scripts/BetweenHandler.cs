using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class BetweenHandler : MonoBehaviour
    {
        [SerializeField] private GameObject parent;
        [SerializeField] private TMP_Text titleDisplay;
        [SerializeField] private string positiveTitleMessage, negativeTitleMessage;
        [SerializeField] private Color positiveTitleColor, negativeTitleColor;

        private Vector3 originalTitleScale;
        private bool win;

        private void Start()
        {
            originalTitleScale = titleDisplay.transform.localScale;
        }

        public void ToggleVisibility(bool visible)
        {
            parent.SetActive(visible);
        }
        
        public void SetLastMicrogameResult(bool won)
        {
            win = won;
            
            titleDisplay.text = win ? positiveTitleMessage : negativeTitleMessage;
            titleDisplay.color = win ? positiveTitleColor : negativeTitleColor;
        }

        public IEnumerator Animate()
        {
            titleDisplay.transform.localScale = originalTitleScale;
            titleDisplay.transform.DOScale(titleDisplay.transform.localScale * 1.5f, 1f);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
