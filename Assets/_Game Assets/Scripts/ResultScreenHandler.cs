using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game_Assets.Scripts
{
    public class ResultScreenHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject parent;
        [SerializeField] private Transform heartsParent;
        
        [Header("UI")]
        [SerializeField] private TMP_Text titleDisplay;
        [SerializeField] private string positiveTitleMessage, negativeTitleMessage;
        [SerializeField] private Color positiveTitleColor, negativeTitleColor;
        
        private Vector3 originalTitleScale;
        private List<Image> heartsList;
        
        private bool lastMicrogameResult;

        private void Start()
        {
            originalTitleScale = titleDisplay.transform.localScale;
            heartsList = heartsParent.GetComponentsInChildren<Image>(true).ToList();
            
            ToggleVisibility(false);
        }

        public void ToggleVisibility(bool visible)
        {
            parent.SetActive(visible);
        }
        
        public bool SetLastMicrogameResult(bool won)
        {
            lastMicrogameResult = won;
            
            titleDisplay.text = lastMicrogameResult ? positiveTitleMessage : negativeTitleMessage;
            titleDisplay.color = lastMicrogameResult ? positiveTitleColor : negativeTitleColor;

            return heartsList.Count - 1 <= 0;
        }

        public IEnumerator Animate()
        {
            titleDisplay.transform.localScale = originalTitleScale;
            titleDisplay.transform.DOScale(titleDisplay.transform.localScale * 1.5f, 1f);
            
            UpdateHearts();
            
            yield return new WaitForSeconds(1.5f);
        }

        private void UpdateHearts()
        {
            if (!lastMicrogameResult)
            {
                heartsList[0].color = Color.black;
                heartsList.RemoveAt(0);
                
                if (heartsList.Count <= 0)
                {
                    Debug.Log("LOST!");
                }
            }
        }
    }
}
