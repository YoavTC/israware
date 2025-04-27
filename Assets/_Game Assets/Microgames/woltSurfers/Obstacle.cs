using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    [SelectionBase,
    RequireComponent(typeof(BoxCollider))]
    public class Obstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.root.GetComponent<Wolter>().OnObstacleHit(transform);
            }
        }

        #region Mesh Bounds Calculation
        [SerializeField, OnValueChanged(nameof(CalculateMeshBounds))] private Vector3 padding;
        
        private Bounds bounds;
        
        private BoxCollider boxCollider;
        private BoxCollider BoxCollider => boxCollider ? boxCollider : GetComponent<BoxCollider>();
        
        private void Awake()
        {
            CalculateMeshBounds();
        }

        [Button]
        private void CalculateMeshBounds()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

            if (meshFilters.Length == 0)
            {
                bounds = new Bounds(transform.position, Vector3.zero);
                return;
            }

            // Initialize bounds with the first MeshFilter converted to world space.
            var firstMesh = meshFilters[0];
            Vector3 firstCenter = firstMesh.transform.TransformPoint(firstMesh.sharedMesh.bounds.center);
            Vector3 firstSize = Vector3.Scale(firstMesh.sharedMesh.bounds.size, firstMesh.transform.lossyScale);
            bounds = new Bounds(firstCenter, firstSize);

            // Loop over remaining mesh filters and encapsulate their world bounds.
            foreach (MeshFilter mf in meshFilters)
            {
                Vector3 worldCenter = mf.transform.TransformPoint(mf.sharedMesh.bounds.center);
                Vector3 worldSize = Vector3.Scale(mf.sharedMesh.bounds.size, mf.transform.lossyScale);
                Bounds worldBounds = new Bounds(worldCenter, worldSize);
                bounds.Encapsulate(worldBounds);
            }

            BoxCollider.center = bounds.center - transform.position;
            BoxCollider.size = bounds.size + new Vector3(padding.x, padding.y, padding.z);
        }
        #endregion
    }
}
