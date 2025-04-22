using System.Linq;
using UnityEngine;
using UnityUtils;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.woltSurfers
{
    public class RandomPropsGroup : MonoBehaviour
    {
        [SerializeField] private bool noneIsAnOption;
        [Space]
        [SerializeField] private bool destroyComponentAfter = true;
        [SerializeField] private bool destroyInactiveChildren = true;
        
        private void Awake()
        {
            Transform[] children = transform.Children().ToArray();
            int randomIndex = Random.Range(0, children.Length + (noneIsAnOption ? 1 : 0));
            
            for (int i = 0; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(i == randomIndex);
            }

            if (destroyInactiveChildren)
            {
                foreach (Transform child in children)
                {
                    if (!child.gameObject.activeSelf)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
            
            if (destroyComponentAfter) Destroy(this);
        }
    }
}
