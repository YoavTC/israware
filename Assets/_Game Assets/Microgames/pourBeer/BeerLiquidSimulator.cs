using UnityEngine;

namespace _Game_Assets.Microgames.pourBeer
{
    public class BeerLiquidSimulator : MonoBehaviour
    {
        [SerializeField] private LineRenderer liquidLineRenderer;
        [SerializeField] private float distance;

        private void Start()
        {
            liquidLineRenderer.positionCount = 2;
        }

        void Update()
        {
            liquidLineRenderer.SetPosition(1, transform.InverseTransformPoint(transform.position + Vector3.down) * distance); 
        }
    }
}
