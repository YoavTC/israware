using System;
using DG.Tweening;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Instantiate(particle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
