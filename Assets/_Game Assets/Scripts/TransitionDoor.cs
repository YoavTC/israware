using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class TransitionDoor : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField, AnimatorParam("animator")] private string openAnimatorParam;
        [SerializeField, AnimatorParam("animator")] private string closeAnimatorParam;

        [SerializeField] private float doorClosedDuration;
        [SerializeField] private bool isPlaying;
        
        public IEnumerator Toggle(Action executeWhenClosedCallback)
        {
            // Close
            isPlaying = true;
            animator.SetTrigger(closeAnimatorParam);
            yield return new WaitUntil(() => !isPlaying);

            // Execute the callback
            executeWhenClosedCallback?.Invoke();
            yield return new WaitForSeconds(doorClosedDuration);
            
            // Open
            animator.SetTrigger(openAnimatorParam);
            yield return new WaitUntil(() => !isPlaying);
        }
        
        // Triggered on the end of both animation clips
        public void FinishedAnimation()
        {
            isPlaying = false;
        }
    }
}
