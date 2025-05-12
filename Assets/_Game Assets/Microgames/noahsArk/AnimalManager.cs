using System.Collections;
using System.Collections.Generic;
using System.Linq;
using External_Packages.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.noahsArk
{
    public class AnimalManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private List<Sprite> animalSprites;
        [SerializeField] private Animal animalPrefab;
        [SerializeField] private GameObject animalClickedParticlePrefab;

        [Header("Spawner Settings")] 
        [SerializeField] private float spawnDelay;
        [SerializeField] private Vector2 spawnMagnitudeOffsetRange;
        [SerializeField] private Vector2[] AITargetPoints;
        
        [Header("Events")]
        [SerializeField] private UnityEvent allAnimalsClickedOnUnityEvent;

        private IEnumerator Start()
        {
            animalSprites.Shuffle();

            WaitForSeconds delay = new WaitForSeconds(spawnDelay);
            
            foreach (Sprite animalSprite in animalSprites)
            {
                SpawnAnimal(animalSprite);
                yield return delay;
            }
        }

        private void SpawnAnimal(Sprite animalSprite)
        {
            Vector2 spawnPosition = Random.insideUnitCircle.normalized * Random.Range(spawnMagnitudeOffsetRange.x, spawnMagnitudeOffsetRange.y);
            Instantiate(animalPrefab, transform)
                .Init(animalSprite, spawnPosition, AITargetPoints, OnAnimalClickedOn);
        }

        public void OnAnimalClickedOn(Animal animal)
        {
            Instantiate(animalClickedParticlePrefab, animal.transform.position, Quaternion.identity);
            Destroy(animal.gameObject);
            
            if (transform.childCount == 1)
            {
                allAnimalsClickedOnUnityEvent?.Invoke();
            }
        }
    }
}
