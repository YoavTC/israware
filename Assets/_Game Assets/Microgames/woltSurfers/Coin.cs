using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class Coin : MoveWithEnvironmentBase
    {
        [SerializeField] private ParticleSystem particle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var particleObject = Instantiate(particle, transform.position, Quaternion.identity);
                particleObject.gameObject.AddComponent<MoveWithEnvironment>();
                Destroy(gameObject);
            }
        }
    }
}
