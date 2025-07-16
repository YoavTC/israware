using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EditorAttributes;
using External_Packages.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.woltFrogger
{
    public class CarSpawner : MonoBehaviour
    {
        [SerializeField] private Transform carPrefab;
        [SerializeField] private float carMaxSpeed;
        [SerializeField] private float carMinSpeed;

        [SerializeField] private Vector2[] leftSpawnPoints;
        [SerializeField] private Vector2[] rightSpawnPoints;

        private List<Vector2> spawnPoints;

        [SerializeField] private float spawnInterval;
        [SerializeField ,ReadOnly] private float timer;

        private void Start()
        {
            spawnPoints = leftSpawnPoints.Concat(rightSpawnPoints).ToList();
            
            SpawnCar();
            SpawnCar();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnCar();
            }
        }
        
        private void SpawnCar()
        {
            if (spawnPoints.Count <= 0) return;
            
            var spawnPoint = spawnPoints.Random();
            var car = Instantiate(carPrefab, spawnPoint, Quaternion.identity, transform);
            
            spawnPoints.Remove(spawnPoint);
            
            Vector3 carScale = car.localScale;
            carScale.x *= -Mathf.Sign(spawnPoint.x);
            car.localScale = carScale;
            
            car.DOMoveX(spawnPoint.x * -1, 
                Random.Range(carMinSpeed, carMaxSpeed))
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Destroy(car.gameObject);
                    spawnPoints.Add(spawnPoint);
                });
        }
    }
}
