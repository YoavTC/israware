using System.Linq;
using _Game_Assets.Microgames.defeatAdolf.Code.Enums;
using _Game_Assets.Scripts.Reusables;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.defeatAdolf.Code
{
    public class UIAttackMenuHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform menuRectTransform;
        [SerializeField] private SerializedDictionary<UIAttackMenuOption, ActionType> attackMenuOptionsDictionary;
        private UIAttackMenuOption[] attackMenuOptions;
        
        [Header("Transition Settings")]
        [SerializeField] private Vector2 inPosition;
        [SerializeField] private Vector2 outPosition;
        [SerializeField] private TweenSettings tweenTransitionSettings;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<ActionType> menuOptionSelectUnityEvent;

        private void Start()
        {
            attackMenuOptions = attackMenuOptionsDictionary.Keys.ToArray();
        }

        public void TransitionMenu(bool transitionIn)
        {
            if (transitionIn) TransitionIn();
            else TransitionOut();
        }
        
        [Button]
        private void TransitionIn()
        {
            menuRectTransform.DOAnchorPos(inPosition, tweenTransitionSettings.duration)
                .SetAs(tweenTransitionSettings.GetParams());
        }
        
        [Button]
        private void TransitionOut()
        {
            menuRectTransform.DOAnchorPos(outPosition, tweenTransitionSettings.duration)
                .SetAs(tweenTransitionSettings.GetParams());
        }

        public void OnMenuOptionSelected(UIAttackMenuOption selectedMenuOption)
        {
            menuOptionSelectUnityEvent?.Invoke(attackMenuOptionsDictionary[selectedMenuOption]);
            Debug.Log("Selected Menu Option: " + attackMenuOptionsDictionary[selectedMenuOption]);
            
            // Disable all menu options
            foreach (UIAttackMenuOption menuOption in attackMenuOptions)
            {
                menuOption.DisableInteraction(menuOption != selectedMenuOption);
            }
        }

        #region Debugging
#if UNITY_EDITOR
        
        [Button]
        private void EnableAllMenuOptions()
        {
            foreach (UIAttackMenuOption menuOption in attackMenuOptions)
            {
                menuOption.EnableInteraction();
            }
        }
        
        [Button]
        private void DisableAllMenuOptions(bool updateStyle)
        {
            foreach (UIAttackMenuOption menuOption in attackMenuOptions)
            {
                menuOption.DisableInteraction(updateStyle);
            }
        }
#endif
        #endregion
    }
}
