using System.Collections.Generic;
using DG.Tweening;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        [Header("Spawning Settings")]
        [SerializeField] private float speed;
        [SerializeField] private float spawnDelayFactor;
        [SerializeField] private float zKillPoint;

        [Header("City Block Settings")]
        [SerializeField] private GameObject[] cityBlockPrefabs;
        [SerializeField] private List<Transform> cityBlocks;

        [Header("City Block Spawn Animation Settings")]
        [SerializeField] private float cityBlockSpawnAnimationDistance;
        [SerializeField] private float cityBlockSpawnAnimationDuration;
        [SerializeField] private Ease cityBlockSpawnAnimationEase;

        private float elapseTime;

        private void Start()
        {
            elapseTime = spawnDelayFactor * speed;
        }

        void Update()
        {
            HandleSpawning();
            HandleMoving();
        }

        private void HandleSpawning()
        {
            elapseTime += Time.deltaTime;

            if (elapseTime >= spawnDelayFactor * speed)
            {
                GameObject cityBlock = Instantiate(cityBlockPrefabs[Random.Range(0, cityBlockPrefabs.Length)],
                    transform.position,
                    // Quaternion.Euler(new Vector3(0, External_Packages.Random.RandomBool() ? 0f : 180f, 0)),
                    Quaternion.identity,
                    transform);
                
                cityBlock.transform.localPosition += Vector3.down * cityBlockSpawnAnimationDistance;

                cityBlock.transform.DOMoveY(transform.position.y, cityBlockSpawnAnimationDuration)
                    .SetEase(cityBlockSpawnAnimationEase);
                
                cityBlocks.Add(cityBlock.transform);
                elapseTime = 0f;
            }
        }

        private void HandleMoving()
        {
            for (int i = cityBlocks.Count - 1; i >= 0; i--)
            {
                Transform cityBlock = cityBlocks[i];
                cityBlock.position += Vector3.back * (speed * Time.deltaTime);

                if (cityBlock.position.z < zKillPoint)
                {
                    cityBlocks.RemoveAt(i);
                    Destroy(cityBlock.gameObject);
                }
            }
        }
        
        public (float, float) GetMoveEnvironmentSettings()
        {
            return (speed, zKillPoint);
        }
    }
}
