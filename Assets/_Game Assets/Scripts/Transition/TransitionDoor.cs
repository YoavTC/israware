using NaughtyAttributes;
using UnityEngine;

namespace _Game_Assets.Scripts.Transition
{
    public class TransitionDoor : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField, AnimatorParam("animator")] private string openAnimatorParam;
        [SerializeField, AnimatorParam("animator")] private string closeAnimatorParam;
        
        [Button] public void Open() => animator.SetTrigger(openAnimatorParam);
        [Button] public void Close() => animator.SetTrigger(closeAnimatorParam);
    }
}
