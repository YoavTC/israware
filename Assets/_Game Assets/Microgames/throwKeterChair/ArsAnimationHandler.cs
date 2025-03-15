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

        public void OnThrowChair(bool hit)
        {
            animator?.SetBool(randomParam, Random.RandomBool());
            animator?.SetTrigger(hit ? hitParam : missParam);
        }
    }
}
