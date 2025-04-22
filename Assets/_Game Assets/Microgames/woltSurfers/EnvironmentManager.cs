using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class EnvironmentManager : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float spawnDelayFactor;
        [SerializeField] private float zKillPoint;

        [SerializeField] private GameObject[] cityBlockPrefabs;
        [SerializeField] private List<Transform> cityBlocks;

        [SerializeField] private float cityBlockSpawnAnimationDistance;
        [SerializeField] private float cityBlockSpawnAnimationDuration;
        [SerializeField] private Ease cityBlockSpawnAnimationEase;

        private float elapseTime;
        
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
                    Quaternion.identity);
                
                cityBlock.transform.localPosition += Vector3.down * cityBlockSpawnAnimationDistance;
                // cityBlock.transform.localScale = Vector3.zero;
                //
                // cityBlock.transform.DOScale(1f, cityBlockSpawnAnimationDuration)
                //     .SetEase(cityBlockSpawnAnimationEase);

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
    }
}
