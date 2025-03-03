using System.IO;
using System.Linq;
using _Game_Assets.Scripts;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class CreateMicrogameEditor : UnityEditor.Editor
    {
        [MenuItem("Tools/Create New Microgame")]
        public static void CreateMicrogame()
        {
            // Prompt for name
            string microgameName = EditorUtility.SaveFilePanel("Enter Microgame Name", "", "NewMicrogame", "");
            if (string.IsNullOrEmpty(microgameName)) return;
            microgameName = Path.GetFileNameWithoutExtension(microgameName);

            // Create ScriptableObject
            string resourcesPath = "Assets/Resources/Microgames/";
            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            MicrogameScriptableObject microgame = ScriptableObject.CreateInstance<MicrogameScriptableObject>();
            microgame.id = microgameName;
            microgame.sceneName = microgameName;
            AssetDatabase.CreateAsset(microgame, resourcesPath + microgameName + ".asset");
            AssetDatabase.SaveAssets();

            // Create new directory
            string microgameDir = "Assets/_Game Assets/Microgames/" + microgameName;
            if (!Directory.Exists(microgameDir))
                Directory.CreateDirectory(microgameDir);

            // Save old scene
            EditorSceneManager.SaveOpenScenes();
            
            // Create new scene
            string scenePath = microgameDir + "/" + microgameName + ".unity";
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, scenePath);
            
            // Add the scene to scene build list
            var buildScenes = EditorBuildSettings.scenes.ToList();
            buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = buildScenes.ToArray();


            // Refresh assets
            AssetDatabase.Refresh();
        }
    }
}