using System;
using _Game_Assets.Scripts.Definitions;
using DG.Tweening;
using External_Packages.Extra_Components;
using External_Packages.MonoBehaviour_Extensions;
using TMPro;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class PromptHandler : Singleton<PromptHandler>, IMicrogameCallbacksListener
    {
        [Header("Settings")]
        [SerializeField] private Language language;
        
        [Header("Components")]
        [SerializeField] private TMP_Text promptDisplay;
        [SerializeField] private TweenScaleEffect tweenScaleEffect;
        
        [Header("Animation Settings")]
        [SerializeField] private float defaultPromptDuration;
        [SerializeField] private Vector2 inOutFadeDuration;
        
        public void OnMicrogameLoaded(MicrogameScriptableObject microgame)
        {
            ShowPrompt((language == Language.ENGLISH) ? microgame.ENGLISH_PROMPT : microgame.HEBREW_PROMPT);
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