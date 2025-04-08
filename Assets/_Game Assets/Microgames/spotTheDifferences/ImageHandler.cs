using System.Linq;
using UnityEngine;
using UnityUtils;

namespace _Game_Assets.Microgames.spotTheDifferences
{
    public class ImageHandler : MonoBehaviour
    {
        [SerializeField] private Transform pointsParent;
        public Transform PointsParent => pointsParent;
        
        private const float X_OFFSET = 955f;

        private void Awake()
        {
            Transform[] children = pointsParent.Children().ToArray();
            for (int i = 0; i < children.Length; i++)
            {
                var childCopy = Instantiate(children[i], pointsParent);
                
                childCopy.localPosition = childCopy.localPosition.Add(x: -X_OFFSET);
                
                Transform childrenParent = new GameObject($"[{i}] points parent").transform;
                childrenParent.SetParent(pointsParent);
                childCopy.SetParent(childrenParent);
                children[i].SetParent(childrenParent);
            }
        }
    }
}