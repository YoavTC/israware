using UnityEngine;

namespace _Game_Assets.Scripts.Definitions
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] protected StateMachine stateMachine;
        [SerializeField] protected GameObject screenParent;

        public abstract void Show(bool won);
        public abstract void Hide();
    }
}