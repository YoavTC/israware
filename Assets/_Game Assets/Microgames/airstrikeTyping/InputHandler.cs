using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.airstrikeTyping
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private RectTransform inputField;
        [SerializeField] private TMP_Text[] textFields;
        [SerializeField] private string defaultText;
        private int currentFieldIndex;
        private bool allowInput;

        [Header("Transition Settings")] 
        [SerializeField] private float transitionDelay;
        [SerializeField] private float inInputFieldYPosition;
        [SerializeField] private float inInputFieldTransitionDuration;
        
        [Header("Events")]
        [SerializeField] private UnityEvent characterDeletedUnityEvent;
        [SerializeField] private UnityEvent characterEnteredUnityEvent;
        [SerializeField] private UnityEvent<bool> inputCompleteUnityEvent;

        private string code;

        private void Start()
        {
            foreach (var textField in textFields)
            {
                textField.text = defaultText;
            }
            
            allowInput = false;
        }

        public void ShowInputField(string newCode)
        {
            code = newCode;
            currentFieldIndex = 0;
            
            allowInput = true;

            inputField.DOAnchorPosY(inInputFieldYPosition, inInputFieldTransitionDuration)
                .SetDelay(transitionDelay);
        }

        void Update()
        {
            if (!allowInput) return;

            if (Input.GetKeyDown(KeyCode.Return) && currentFieldIndex == 4)
            {
                SubmitCode();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace) && currentFieldIndex > 0)
            {
                currentFieldIndex--;
                textFields[currentFieldIndex].text = defaultText;
                
                characterDeletedUnityEvent?.Invoke();
            }
            else if (currentFieldIndex < 4 && Input.inputString.Length > 0)
            {
                var character = Input.inputString[0];

                if (char.IsNumber(character) || char.IsLetter(character))
                {
                    SetField(char.ToString(character).ToUpper());
                }
            }
        }

        private void SetField(string character)
        {
            textFields[currentFieldIndex].text = character;
            currentFieldIndex++;
            
            characterEnteredUnityEvent?.Invoke();
        }
        
        private void SubmitCode()
        {
            allowInput = false;
            string typedCode = String.Empty;

            foreach (var textField in textFields)
            {
                typedCode += textField.text;
            }
            
            bool isCorrect = typedCode.Trim() == code.Trim();
            inputCompleteUnityEvent?.Invoke(isCorrect);
            
            inputField.DOAnchorPosY(-300f, inInputFieldTransitionDuration);
        }  
    }
}
