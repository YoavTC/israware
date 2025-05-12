using System;
using DG.Tweening;
using External_Packages.Extensions;
using External_Packages.Extra_Components;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.noahsArk
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TweenScaleEffect scaleEffect;
        
        [Header("Movement AI Settings")]
        [SerializeField] private Vector2 randomMovementSpeedRange;
        [SerializeField] private Vector2 randomTargetPointOffsetRange;

        private Vector2[] targetPoints;
        private Action<Animal> onAnimalClickedOn;
        
        public void Init(Sprite sprite, Vector2 spawnPosition, Vector2[] _targetPoints, Action<Animal> _onAnimalClickedOn)
        {
            targetPoints = _targetPoints;
            onAnimalClickedOn = _onAnimalClickedOn;
            
            spriteRenderer.sprite = sprite;
            scaleEffect.DoEffect();

            transform.position = spawnPosition;
            MoveAnimal();
        }

        private void MoveAnimal()
        {
            transform.DOMove(
                    targetPoints.Random() *
                    Random.Range(randomTargetPointOffsetRange.x, randomTargetPointOffsetRange.y),
                    Random.Range(randomMovementSpeedRange.x, randomMovementSpeedRange.y))
                .SetEase(Ease.Linear)
                .OnComplete(MoveAnimal);
        }

        public void OnClickedOn()
        {
            onAnimalClickedOn?.Invoke(this);
        }
    }
}
