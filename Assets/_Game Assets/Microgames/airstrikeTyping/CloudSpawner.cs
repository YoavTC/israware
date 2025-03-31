using System;
using DG.Tweening;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;

namespace _Game_Assets.Microgames.airstrikeTyping
{
    public class CloudSpawner : CooldownAction
    {
        [SerializeField] private Sprite[] clouds;
        [SerializeField] private GameObject cloudPrefab;
        
        [SerializeField] private float spawnCooldown;
        
        [SerializeField] private Vector2 minMaxStartCloudYPosition;
        [SerializeField] private Vector2 startEndCloudXPosition;
        [SerializeField] private float cloudMoveDuration;
        
        private void Start()
        {
            ActionEnabled = true;
            ActionCooldown = spawnCooldown;
        }

        protected override void ExecuteAction()
        {
            Vector2 startPosition = new Vector2(startEndCloudXPosition.x, UnityEngine.Random.Range(minMaxStartCloudYPosition.x, minMaxStartCloudYPosition.y));
            Transform cloud = Instantiate(cloudPrefab, startPosition, Quaternion.identity, transform).transform;
            cloud.GetComponent<SpriteRenderer>().sprite = clouds[UnityEngine.Random.Range(0, clouds.Length)];

            cloud.DOMoveX(startEndCloudXPosition.y, cloudMoveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(cloud.gameObject));
        }
    }
}
