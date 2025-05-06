using _Game_Assets.Microgames.defeatAdolf.Code.Enums;
using EditorAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game_Assets.Microgames.defeatAdolf.Code
{
    public class UIAttackMenuOption : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Pointer Settings")] 
        [SerializeField, EnumButtons] private PointerEventData.InputButton acceptableInputButtons;
        
        [Header("Components")]
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;
        
        [Header("Elements Style Settings")]
        [SerializeField] private Color normalColor;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color disabledColor;
        
        [Header("Button Style Settings")]
        [SerializeField] private Color buttonNormalColor;
        [SerializeField] private Color buttonHoverColor;
        [SerializeField] private Color buttonSelectedColor;
        [SerializeField] private Color buttonDisabledColor;
        
        [SerializeField, HideInEditMode] private StyleState currentStyleState;
        [SerializeField, HideInEditMode] private bool isDisabled;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<UIAttackMenuOption> buttonHoverUnityEvent;
        [SerializeField] private UnityEvent<UIAttackMenuOption> buttonSelectUnityEvent;

        private void Awake() => EnableInteraction();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            // Filter out non-acceptable input buttons
            if (eventData.button == acceptableInputButtons)
            {
                SetStyle(StyleState.SELECTED);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => SetStyle(StyleState.HOVER);
        public void OnPointerExit(PointerEventData eventData) => SetStyle(currentStyleState);
        
        [Button("Disable Interaction")]
        public void DisableInteraction(bool updateStyle = true)
        {
            if (updateStyle) SetStyle(StyleState.DISABLED);
            isDisabled = true;
        }
        
        [Button("Enable Interaction")]
        public void EnableInteraction()
        {
            isDisabled = false;
            SetStyle(StyleState.NORMAL);
        }

        private void SetStyle(StyleState styleState)
        {
            if (isDisabled) return;
            
            // Prevent hover changing style state if the button is selected or disabled
            if (styleState == StyleState.HOVER && currentStyleState is StyleState.SELECTED or StyleState.DISABLED)
            {
                return;
            }
            
            if (styleState != StyleState.HOVER) currentStyleState = styleState;
            
            switch (styleState)
            {
                case StyleState.NORMAL:
                    background.color = buttonNormalColor;
                    icon.color = normalColor;
                    text.color = normalColor;
                    break;
                case StyleState.HOVER:
                    buttonHoverUnityEvent?.Invoke(this);
                    
                    background.color = buttonHoverColor;
                    icon.color = hoverColor;
                    text.color = hoverColor;
                    break;
                case StyleState.SELECTED:
                    buttonSelectUnityEvent?.Invoke(this);
                    
                    background.color = buttonSelectedColor;
                    icon.color = selectedColor;
                    text.color = selectedColor;
                    break;
                case StyleState.DISABLED:
                    background.color = buttonDisabledColor;
                    icon.color = disabledColor;
                    text.color = disabledColor;
                    break;
            }
        }
    }
}
