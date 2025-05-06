using System;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using Void = EditorAttributes.Void;

namespace _Game_Assets.Scripts.Reusables
{
    [Serializable]
    public struct TweenSettings
    {
        public float delay;
        public float duration;
        public Ease ease;
        
        [FoldoutGroup("On Complete", nameof(invokeEventOnComplete), nameof(onComplete))] 
        [SerializeField] private Void completeFoldout;
        [SerializeField, HideInInspector] public bool invokeEventOnComplete;
        [SerializeField, HideInInspector] public UnityEvent onComplete;

        [FoldoutGroup("Loops", nameof(loops), nameof(loopType))] 
        [SerializeField] private Void loopsFoldout;
        [SerializeField, HideInInspector] public int loops;
        [SerializeField, HideInInspector] public LoopType loopType;

        public TweenSettings(float delay, float duration, Ease ease, UnityEvent onComplete = null, LoopType loopType = LoopType.Restart)
        {
            this.delay = delay;
            this.duration = duration;
            this.ease = ease;
            
            this.onComplete = onComplete;
            this.loopType = loopType;

            invokeEventOnComplete = true;
            loops = 1;
        }

        public TweenParams GetParams()
        {
            TweenParams tweenParams = new TweenParams();
            
            tweenParams.SetDelay(delay);
            tweenParams.SetEase(ease);
            tweenParams.SetLoops(loops, loopType);

            if (invokeEventOnComplete)
            {
                UnityEvent completeEvent = onComplete;
                tweenParams.OnComplete(() => completeEvent?.Invoke());
            }

            return tweenParams;
        }
    }
}