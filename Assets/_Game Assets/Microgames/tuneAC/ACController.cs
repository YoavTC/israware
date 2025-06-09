using System.Collections;
using EditorAttributes;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.tuneAC
{
    public class ACController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform emojiDisplayParent;
        [SerializeField] private Animator introAnimator;

        [Header("Intro Settings")] 
        [SerializeField] private float introDelay;
        
        [Header("Settings")]
        [SerializeField] private Vector2Int initialDegreesRange;
        [SerializeField] private Vector2Int initialDegreesOffsetRange;

        [Header("Events")]
        [SerializeField] private UnityEvent<string> degreesChangedUnityEvent;
        [SerializeField] private UnityEvent correctDegreesUnityEvent;
        
        [SerializeField, HideInEditMode] private int initialDegrees;
        [SerializeField, HideInEditMode] private int currentDegrees;

        private IEnumerator Start()
        {
            int randomDirection = External_Packages.Random.RandomIntSign();
            
            emojiDisplayParent.GetChild((1 - (randomDirection * -1)) / 2).gameObject.SetActive(true);
            
            initialDegrees = Random.Range(initialDegreesRange.x, initialDegreesRange.y);
            currentDegrees = initialDegrees + (randomDirection * Random.Range(initialDegreesOffsetRange.x, initialDegreesOffsetRange.y));
            InvokeDegreesChangedEvent();

            yield return new WaitForSeconds(introDelay);

            introAnimator.enabled = true;
        }

        [UsedImplicitly] // Called from the UI
        public void ChangeDegrees(bool increase)
        {
            currentDegrees += increase ? 1 : -1;
            InvokeDegreesChangedEvent();

            if (currentDegrees == initialDegrees)
            {
                correctDegreesUnityEvent?.Invoke();
            }
        }

        private void InvokeDegreesChangedEvent()
        {
            degreesChangedUnityEvent.Invoke(currentDegrees.ToString());
        }
    }
}
