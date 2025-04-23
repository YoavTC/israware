using System.Collections;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Animator scooterAnimator;
        
        [Header("Lane Switch")]
        [SerializeField] private float laneCenterXPosition;
        [SerializeField] private float laneSwitchCooldown;
        [SerializeField, ReadOnly] private float timeSinceLastLaneSwitch;
        [SerializeField] private bool onRightLane;
        [Space]
        [SerializeField] private float laneAnimationDuration;
        [SerializeField] private Ease laneAnimationEase;

        [Header("Action Move")] 
        [SerializeField] private float actionMoveStayDuration;
        [SerializeField] private float actionMoveCooldown;
        [SerializeField, ReadOnly] private float timeSinceLastActionMove;

        [Header("Action Move Animations")] 
        [SerializeField, AnimatorParamDropdown(nameof(scooterAnimator))] private string slideAnimationParameter;
        [SerializeField, AnimatorParamDropdown(nameof(scooterAnimator))] private string jumpAnimationParameter;
        
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";

        private void Update()
        {
            HandleActionMoves();
            HandleLaneSwitching();

            timeSinceLastActionMove += Time.deltaTime;
            timeSinceLastLaneSwitch += Time.deltaTime;
        }

        private void HandleActionMoves()
        {
            int moveDirection = (int) Input.GetAxisRaw(VERTICAL);
            
            bool canPerformActionMove = moveDirection != 0 && timeSinceLastActionMove >= actionMoveCooldown;
            if (canPerformActionMove)
            {
                StartCoroutine(PerformActionMoveCoroutine(moveDirection == 1));
                timeSinceLastActionMove = 0f;
            }
        }

        private IEnumerator PerformActionMoveCoroutine(bool isJumping)
        {
            NotifyAnimator(isJumping, true);
            yield return new WaitForSeconds(actionMoveStayDuration);
            NotifyAnimator(isJumping, false);
        }

        private void NotifyAnimator(bool isJumping, bool state)
        {
            scooterAnimator.SetBool(
                isJumping ? jumpAnimationParameter : slideAnimationParameter,
                state
                );
        }

        private void HandleLaneSwitching()
        {
            int moveDirection = (int) Input.GetAxisRaw(HORIZONTAL);

            bool canSwitchLanes = moveDirection != 0 && (onRightLane ? moveDirection == -1 : moveDirection == 1);
            if (canSwitchLanes && timeSinceLastLaneSwitch >= laneSwitchCooldown)
            {
                SwitchLanes(moveDirection);
                timeSinceLastLaneSwitch = 0f;
            }
        }

        private void SwitchLanes(int direction)
        {
            playerTransform.DOMoveX(laneCenterXPosition * direction, laneAnimationDuration)
                .SetEase(laneAnimationEase);

            onRightLane = direction == 1;
        }
    }
}
