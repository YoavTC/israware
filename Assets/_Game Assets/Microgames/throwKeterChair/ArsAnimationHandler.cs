using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEditor.Animations;
using UnityEngine;
using Random = External_Packages.Random;

namespace _Game_Assets.Microgames.throwKeterChair
{
    public class ArsAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField, AnimatorParam(nameof(animator))] private string hitParam, missParam;
        [SerializeField, AnimatorParam(nameof(animator))] private string randomParam;

        [SerializeField] private Animator ars2Animator;
        [SerializeField, AnimatorParam(nameof(ars2Animator))] private string ars2HitParam;

        [SerializeField] private MMF_Player feedbackPlayer, chairFeedbackPlayer;

        public void OnThrowChair(bool hit)
        {
            animator?.SetBool(randomParam, Random.RandomBool());
            animator?.SetTrigger(hit ? hitParam : missParam);
            
            if (hit) ars2Animator?.SetTrigger(ars2HitParam);
        }

        [UsedImplicitly] // Triggered by animation
        public void PlayCameraShakeFeedback(float strength)
        {
            feedbackPlayer.GetFeedbackOfType<MMF_CameraShake>().CameraShakeProperties.AmplitudeX = strength;
            feedbackPlayer.GetFeedbackOfType<MMF_CameraShake>().CameraShakeProperties.AmplitudeY = strength;
            feedbackPlayer.GetFeedbackOfType<MMF_CameraShake>().CameraShakeProperties.AmplitudeZ = strength;
            feedbackPlayer.PlayFeedbacks();
        }

        [UsedImplicitly] // Triggered by animation
        public void PlayChairFeedback()
        {
            chairFeedbackPlayer.PlayFeedbacks();
        }
    }
}
