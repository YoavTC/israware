using System.IO;
using System.Linq;
using _Game_Assets.Scripts.Definitions;
using Mono.Cecil;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    public class CreateMicrogameEditor : UnityEditor.Editor
    {
        private const string MICROGAME_SCENE_TEMPLATE_PATH = "Assets/Presets/DefaultMicrogameSceneTemplate.unity";
        private const string MICROGAME_ASMDEF_TEMPLATE_PATH = "Assets/Presets/AssemblyDefinitionTemplate";
        
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
            AssetDatabase.CopyAsset(MICROGAME_SCENE_TEMPLATE_PATH, scenePath);
            var newScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, scenePath);
            
            // Add the scene to scene build list
            var buildScenes = EditorBuildSettings.scenes.ToList();
            buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = buildScenes.ToArray();
            
            // Create an Assembly Definition file in the new directory
            string asmdefPath = microgameDir + "/" + microgameName + ".asmdef";
            AssetDatabase.CopyAsset(MICROGAME_ASMDEF_TEMPLATE_PATH, asmdefPath);
            
            // Replace the placeholder in the asmdef file
            var asmdefPreset = AssetDatabase.LoadAssetAtPath<TextAsset>(asmdefPath);
            string asmdef = asmdefPreset.text.Replace("[MICROGAME_NAME]", microgameName);
            File.WriteAllText(asmdefPath, asmdef);
            
            // Refresh assets
            AssetDatabase.Refresh();
        }
    }
}