using System.Collections.Generic;
using External_Packages.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.dontTouchWomen
{
    public class WomenManager : MonoBehaviour
    {
        [SerializeField] private Transform hasidicTransform;
        
        [SerializeField] private Woman womanPrefab;
        [SerializeField] private Vector2 speedRange;
        [SerializeField] private float touchDistance;
        [SerializeField] private List<Woman> womenList = new List<Woman>();

        [SerializeField] private int spawnCount;
        [SerializeField] private List<Vector2> spawnPositions;

        private void Start()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Woman woman = Instantiate(womanPrefab, GetRandomSpawnPosition(), Quaternion.identity, transform);
                woman.Init(hasidicTransform, Random.Range(speedRange.x, speedRange.y), touchDistance);
                womenList.Add(woman);
            }
        }

        private Vector2 GetRandomSpawnPosition()
        {
            Vector2 randomPosition = spawnPositions.Random();
            spawnPositions.Remove(randomPosition);
            return randomPosition;
        }

        public void StopAllWomen()
        {
            foreach (Woman woman in womenList)
            {
                woman.StopWoman();
            }
        }
    }
}
