using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    public abstract class MoveWithEnvironmentBase : MonoBehaviour
    {
        private EnvironmentManager environmentManager;
        private float speed;
        private float zKillPoint;
        
        private void Awake()
        {
            (float, float) settings = EnvironmentManager.Instance.GetMoveEnvironmentSettings();
            
            speed = settings.Item1;
            zKillPoint = settings.Item2;
        }
        
        void Update()
        {
            transform.position += Vector3.back * (speed * Time.deltaTime);

            if (transform.position.z < zKillPoint)
            {
                Destroy(transform.gameObject);
            }
        }
    }
}
