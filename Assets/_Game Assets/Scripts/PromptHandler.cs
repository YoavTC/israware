using System;
using System.Collections;
using _Game_Assets.Scripts.Definitions;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using External_Packages.Extra_Components;
using External_Packages.MonoBehaviour_Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game_Assets.Scripts
{
    public class PromptHandler : Singleton<PromptHandler>, IMicrogameCallbacksListener
    {
        [Header("Settings")]
        [SerializeField] private Language language;
        
        [Header("Components")]
        [SerializeField] private TMP_Text promptDisplay;
        [SerializeField] private Image promptVisualDisplay;
        [SerializeField] private TweenScaleEffect tweenScaleEffect;
        
        [Header("Animation Settings")]
        [SerializeField] private float defaultPromptDuration;
        [SerializeField] private Vector2 inOutFadeDuration;
        
        [Header("Prompt Visual Settings")]
        [SerializeField] private SerializedDictionary<PromptVisual, Sprite[]> promptVisualsDictionary;
        [SerializeField] private float flickerDuration;
        [SerializeField] private float moveDistance;
        [SerializeField] private float moveDuration;
        [SerializeField] private Ease moveEase;
        
        public void OnMicrogameLoaded(MicrogameScriptableObject microgame)
        {
            ShowPrompt((language == Language.ENGLISH) ? microgame.ENGLISH_PROMPT : microgame.HEBREW_PROMPT);
            // ShowPromptVisual(microgame.promptVisual);
            StartCoroutine(ShowPromptVisual(microgame.promptVisual));
        }

        public void OnMicrogameFinished(bool result)
        {
            Debug.Log("Killing prompt");
            
            tweenScaleEffect.DOKill(true);
            promptDisplay.DOKill(true);
            HidePrompt();
        }

        private void Start()
        {
            UpdateLanguagePreference(language);
        }
        
        public void UpdateLanguagePreference(Language newLanguage)
        {
            language = newLanguage;
            promptDisplay.isRightToLeftText = (newLanguage == Language.HEBREW);
        }
        
        private void ShowPrompt(string text) => ShowPrompt(text, Color.white);
        private void ShowPrompt(string text, Color color)
        {
            Debug.Log($"Showing prompt '{text}'");
            
            promptDisplay.color = color;
            promptDisplay.text = text;
            promptDisplay.alpha = 0f;
            promptDisplay.enabled = true;

            promptDisplay.DOFade(1f, inOutFadeDuration.x);
            
            tweenScaleEffect.DoEffect();
            
            promptDisplay.DOFade(0f, inOutFadeDuration.y)
                .SetDelay(defaultPromptDuration)
                .OnComplete(HidePrompt);
            promptVisualDisplay.DOFade(0f, inOutFadeDuration.y)
                .SetDelay(defaultPromptDuration);
        }

        private IEnumerator ShowPromptVisual(PromptVisual promptVisual)
        {
            if (promptVisual == PromptVisual.NONE) yield break;
                
            promptVisualDisplay.DOFade(1f, inOutFadeDuration.x);
            Sprite[] sprites = promptVisualsDictionary[promptVisual];
            
            if (sprites.Length > 1)
            {
                promptVisualDisplay.sprite = sprites[0];
                
                int i = 1;
                while (promptDisplay.enabled)
                {
                    yield return new WaitForSeconds(flickerDuration);
                    promptVisualDisplay.sprite = sprites[i];
                    i ^= 1;
                }
            } else promptVisualDisplay.sprite = sprites[0];
            
            
            if (promptVisual == PromptVisual.MOUSE_MOVE)
            {
                float initialXPosition = promptVisualDisplay.rectTransform.anchoredPosition.x;
                
                promptVisualDisplay.rectTransform.DOAnchorPosX(moveDistance, moveDuration)
                    .SetEase(moveEase)
                    .SetLoops(5, LoopType.Yoyo)
                    .OnComplete(() => promptVisualDisplay.rectTransform.DOAnchorPosX(initialXPosition, 0f));
            }
        }

        private void HidePrompt()
        {
            promptDisplay.enabled = false;
            promptDisplay.text = String.Empty;
            promptDisplay.alpha = 1f;
            promptDisplay.transform.localScale = Vector3.one;
        }
    }
}