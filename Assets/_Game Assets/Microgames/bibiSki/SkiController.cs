using System;
using System.Collections;
using DG.Tweening;
using EditorAttributes;
using External_Packages;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.bibiSki
{
    public class SkiController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform keybindRectTransform;
        [SerializeField] private RectTransform timeCircleRectTransform;

        [Header("Settings")]
        [SerializeField] private float keybindShowDelay;
        [Space]
        [SerializeField] private Vector2 timeCircleStartEndScale;
        [SerializeField] private float timeCircleDelay;
        [SerializeField] private float timeCircleDuration;
        
        [Header("Time")]
        [SerializeField] private Vector2 timeWindowStartEnd;
        [SerializeField] private bool timeWithinWindow;
        [SerializeField, ReadOnly] private float time;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<string> selectRandomKeybindUnityEvent;
        [SerializeField] private UnityEvent<bool> pressedKeybindUnityEvent;

        private char randomChar;
        private KeyCode randomKeyCode;
        private bool pressedKeybind;

        private IEnumerator Start()
        {
            // Generate & assign random keybind
            randomChar = (char)Random.Range('A', 'Z' + 1);
            if (randomChar == 'O') randomChar = 'A';
            if (randomChar == 'R') randomChar = 'Z';

            randomKeyCode = CharToKeyCode(randomChar);
            
            // Notify keybind display
            selectRandomKeybindUnityEvent?.Invoke(randomChar.ToString());

            // Animate circle
            timeCircleRectTransform?.DOScale(Vector3.one * timeCircleStartEndScale.x, 0f);
            timeCircleRectTransform?.DOScale(Vector3.one * timeCircleStartEndScale.y, timeCircleDuration)
                .SetDelay(timeCircleDelay)
                .OnComplete(() =>
                {
                    if (!pressedKeybind) 
                    {
                        pressedKeybindUnityEvent?.Invoke(false);
                    }
                });
            
            // Fade out every UI child of keybindRectTransform
            Graphic[] uiChildrenArray = HelperFunctions.GetAllComponentsInChildren<Graphic>(keybindRectTransform).ToArray();
            FadeUIElements(uiChildrenArray, 0f, 0f);
            
            yield return new WaitForSeconds(keybindShowDelay);
            
            // Fade in keybindRectTransform after delay
            FadeUIElements(uiChildrenArray, 1f, 0.5f);
        }

        private void FadeUIElements(Graphic[] elements, float alpha, float duration)
        {
            foreach (Graphic element in elements)
            {
                element.DOFade(alpha, duration);
            }
        }
        
        public static KeyCode CharToKeyCode(char character)
        {
            string keyString = character.ToString().ToUpper();
            if (Enum.TryParse(keyString, out KeyCode keyCode))
            {
                return keyCode;
            }
            
            Debug.LogWarning("Failed to convert character " + character + " to a KeyCode.");
            return KeyCode.None;
        }

        private void Update()
        {
            if (pressedKeybind) return;
            
            time += Time.deltaTime;
            
            timeWithinWindow = time > timeWindowStartEnd.x && time < timeWindowStartEnd.y;
            
            if (timeWithinWindow && Input.GetKeyDown(randomKeyCode))
            {
                pressedKeybind = true;
                pressedKeybindUnityEvent?.Invoke(true);
            }
        }
    }
}
