using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers.Obstacles.Cars
{
    public class MassConvertProps : MonoBehaviour
    {
        
        [MenuItem("Tools/Mass Convert Cars")]
        public static void ProcessPrefabs()
        {
            Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game Assets/Microgames/woltSurfers/Materials/Props MAT.mat");
            
            // Prompt the user to select a folder
            string folderPath = EditorUtility.OpenFolderPanel("Select Folder Containing Prefabs", "Assets", "");
            if (string.IsNullOrEmpty(folderPath)) return;

            // Convert the folder path to a relative path
            folderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);
            
            // Get all prefab files in the folder
            string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);

            foreach (string prefabPath in prefabPaths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null) continue;

                // Create an instance of the prefab to modify
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if (instance != null)
                {
                    // Process the prefab recursively
                    ProcessGameObject(instance, newMaterial);

                    // Apply changes back to the prefab
                    PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
                    GameObject.DestroyImmediate(instance);
                }
            }
            Debug.Log("Prefab processing completed."); 
        }

        private static void ProcessGameObject(GameObject obj, Material newMaterial)
        {
            // Remove MeshCollider if it exists
            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                GameObject.DestroyImmediate(meshCollider);
            }

            // Set the material of MeshRenderer if it exists
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = newMaterial;
            }

            // Recursively process child objects
            foreach (Transform child in obj.transform)
            {
                ProcessGameObject(child.gameObject, newMaterial);
            }
        }
    }
}