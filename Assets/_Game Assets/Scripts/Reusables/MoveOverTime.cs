using System;
using System.Collections;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Scripts.Reusables
{
    public class MoveOverTime : MonoBehaviour
    {
        private Action moveAction;
        private float elapsedTime;
        private Vector3 originalPosition;
        private RectTransform rectTransform;
        private bool invokeEveryFrame;
        
        [Header("Initialization")] 
        [SerializeField] private bool activateOnAwake;
        [SerializeField] private float activateDelay;
        
        [Header("Destruction")]
        [SerializeField] private bool destroyOnComplete;
        [SerializeField, ShowField(nameof(destroyOnComplete))] private float lifetime;
        [SerializeField] private float destroyDelay;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private MovementType movementType;
        [SerializeField, EnableField(nameof(UseDOTween))] private Ease movementEasing; 
        private bool UseDOTween => movementType == MovementType.DOTween_DOMove || movementType == MovementType.DOTween_DOAnchorPos;
        
        [Header("Update")]
        [SerializeField] private UpdateType updateType;
        [SerializeField] private bool useTimeDeltaTime;

        [Header("Events")] 
        [SerializeField] private bool invokeEventOnStart;
        [SerializeField] private bool invokeEventOnComplete;
        
        [SerializeField, EnableField(nameof(invokeEventOnStart))] private UnityEvent movingStartedUnityEvent;
        [SerializeField, EnableField(nameof(invokeEventOnComplete))] private UnityEvent movingCompletedUnityEvent;

        private enum MovementType
        {
            [InspectorName("Transform.position +=")]
            Transform_PositionIncrement,
            [InspectorName("Transform.Translate")] Transform_Translate,

            [InspectorName("Vector3.MoveTowards")] Vector3_MoveTowards,
            [InspectorName("Vector3.Lerp")] Vector3_Lerp,

            [InspectorName("DOTween DOMove")] DOTween_DOMove,
            [InspectorName("DOTween DOAnchorPos")] DOTween_DOAnchorPos,
        }

        private IEnumerator Start()
        {
            if (activateOnAwake)
            {
                yield return new WaitForSeconds(activateDelay);
                StartMoving();
            }
        }

        public void StartMoving()
        {
            moveAction = null;
            invokeEveryFrame = !UseDOTween;
            
            switch (movementType)
            {
                case MovementType.Transform_PositionIncrement:
                    moveAction = () => transform.position += moveDirection * moveSpeed * (useTimeDeltaTime ? Time.deltaTime : 1f);
                    break;
                case MovementType.Transform_Translate:
                    moveAction = () => transform.Translate(moveDirection * moveSpeed * (useTimeDeltaTime ? Time.deltaTime : 1f));
                    break;
                case MovementType.Vector3_MoveTowards:
                    moveAction = () => transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, moveSpeed * (useTimeDeltaTime ? Time.deltaTime : 1f));
                    break;
                case MovementType.Vector3_Lerp:
                    moveAction = () => transform.position = Vector3.Lerp(transform.position, transform.position + moveDirection, moveSpeed * (useTimeDeltaTime ? Time.deltaTime : 1f));
                    break;
                case MovementType.DOTween_DOMove:
                    moveAction = () => transform.DOMove(moveDirection, moveSpeed).SetUpdate(updateType).SetEase(movementEasing).OnComplete(CompleteCallback);
                    break;
                case MovementType.DOTween_DOAnchorPos:
                    rectTransform = GetComponent<RectTransform>();
                    moveAction = () => rectTransform.DOAnchorPos(moveDirection, moveSpeed).SetUpdate(updateType).SetEase(movementEasing).OnComplete(CompleteCallback);
                    break;
            }
            
            if (moveAction == null) return;

            if (!invokeEveryFrame) moveAction.Invoke();
            if (invokeEventOnStart) movingStartedUnityEvent?.Invoke();
            
            elapsedTime = 0f;
            originalPosition = transform.position;
        }
        
        void Update()
        {
            if (invokeEveryFrame && moveAction != null)
            {
                moveAction.Invoke();
                
                elapsedTime += Time.deltaTime;
                if (elapsedTime > lifetime) CompleteCallback();
            }
        }

        private void CompleteCallback()
        {
            moveAction = null;
            
            if (invokeEventOnComplete) movingCompletedUnityEvent?.Invoke();
            
            if (destroyOnComplete)
            {
                Destroy(gameObject, destroyDelay);
            }
        }
    }
}
