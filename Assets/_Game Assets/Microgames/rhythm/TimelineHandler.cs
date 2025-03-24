using System.Linq;
using UnityEngine;

namespace _Game_Assets.Microgames.rhythm
{
    public class TimelineHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private AudioClip failClip;
        private int clipsIndex;
        [SerializeField] private float speed;
        
        private const float KEY_HITBOX_SIZE = 0.5f;
        private readonly int[] LINES_X_POSITIONS = {-6, -3, 0, 3, 6};

        void Update()
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        
        public void OnKeyPressed(int keyIndex)
        {
            var colliders = Physics.OverlapBox(new Vector3(LINES_X_POSITIONS[keyIndex], 0f, 0f), Vector3.one * KEY_HITBOX_SIZE, Quaternion.identity);
            colliders = colliders.Where(a => a.gameObject.CompareTag("NotPlayer")).ToArray();
            
            if (colliders.Length == 0)
            {
                Debug.Log("Fail!");
                audioSource.clip = failClip;
                audioSource.Play();
                return;
            }
            
            foreach (var tempCollider in colliders)
            {
                if (tempCollider.gameObject.CompareTag("NotPlayer"))
                {
                    tempCollider.enabled = false;
                    audioSource.clip = clips[clipsIndex++];
                    audioSource.Play();
                }
            }
        }
    }
}
