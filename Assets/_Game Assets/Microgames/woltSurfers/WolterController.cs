using DG.Tweening;
using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class WolterController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform playerTransform;
        
        [Header("Lane Switch")]
        [SerializeField] private float laneCenterXPosition;
        [Space]
        [SerializeField] private float laneSwitchCooldown;
        [SerializeField, ReadOnly] private float timeSinceLastLaneSwitch;
        [SerializeField] private bool onRightLane;
        [Space]
        [SerializeField] private float laneAnimationDuration;
        [SerializeField] private Ease laneAnimationEase;

        [Header("Action Move")] 
        [SerializeField] private float jumpHeightYPosition;
        [SerializeField] private float rollHeightYPosition;
        [SerializeField] private float actionMoveStayDuration;
        [Space]
        [SerializeField] private float actionMoveCooldown;
        [SerializeField, ReadOnly] private float timeSinceLastActionMove;
        [Space]
        [SerializeField] private float actionMoveAnimationDuration;
        [SerializeField] private Ease actionMoveAnimationEase;
        
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
                PerformActionMove(moveDirection);
                timeSinceLastActionMove = 0f;
            }
        }

        private void PerformActionMove(int direction)
        {
            float targetYPosition = direction == 1 ? jumpHeightYPosition : rollHeightYPosition;
            float originalYPosition = playerTransform.position.y;
            
            playerTransform.DOMoveY(targetYPosition, actionMoveAnimationDuration)
                .SetEase(actionMoveAnimationEase)
                .OnComplete(() =>
                {
                    playerTransform.DOMoveY(originalYPosition,
                        actionMoveAnimationDuration)
                        .SetDelay(actionMoveStayDuration)
                        .SetEase(actionMoveAnimationEase);
                });
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
