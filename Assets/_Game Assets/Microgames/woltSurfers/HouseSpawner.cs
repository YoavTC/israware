using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class HouseSpawner : MonoBehaviour
    {
        [SerializeField] private bool spawnerActive;
        
        [Header("Components")]
        [SerializeField] private Transform housesSpawnParent;
        
        [Header("Spawn Settings")] 
        [SerializeField] private Vector3 spawnRotation;
        
        [SerializeField] private GameObject[] housePrefabs;
        [SerializeField] private GameObject[] bigHousePrefabs;
        
        [SerializeField] private float houseSpawnInterval;
        [SerializeField] private float bigHouseSpawnInterval;

        [SerializeField] private float houseSpawnAnimationDuration;
        [SerializeField] private Ease houseSpawnAnimationEase;
        
        private GameObject lastSpawnedHousePrefab;
        
        IEnumerator Start()
        {
            spawnerActive = true;
            
            bool bigHouse = External_Packages.Random.RandomBool();
            WaitForSeconds bigHouseDelay = new WaitForSeconds(bigHouseSpawnInterval);
            WaitForSeconds houseDelay = new WaitForSeconds(houseSpawnInterval);

            lastSpawnedHousePrefab = null;
            
            while (spawnerActive)
            {
                bigHouse = External_Packages.Random.RandomBool();
                SpawnHouse(bigHouse);
                yield return bigHouse ? bigHouseDelay : houseDelay;
            }
        }
        
        private void SpawnHouse(bool bigHouse)
        {
            GameObject randomHousePrefab = GetRandomHousePrefabRecursive(bigHouse);
            
            var house = Instantiate(randomHousePrefab,
                transform.position,
                Quaternion.Euler(spawnRotation),
                housesSpawnParent);
            
            house.transform.localScale = Vector3.zero;
            
            house.transform.DOScale(1f, houseSpawnAnimationDuration)
                .SetEase(houseSpawnAnimationEase);
            
            house.transform.DOMoveZ(-10f, 2f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Destroy(house);
                });
        }

        private GameObject GetRandomHousePrefabRecursive(bool bigHouse)
        {
            GameObject randomHousePrefab = bigHouse
                ? bigHousePrefabs[Random.Range(0, bigHousePrefabs.Length)]
                : housePrefabs[Random.Range(0, housePrefabs.Length)];

            if (randomHousePrefab == lastSpawnedHousePrefab)
            {
                randomHousePrefab = GetRandomHousePrefabRecursive(bigHouse);
            }

            lastSpawnedHousePrefab = randomHousePrefab;
            return randomHousePrefab;
        }
    }
}
