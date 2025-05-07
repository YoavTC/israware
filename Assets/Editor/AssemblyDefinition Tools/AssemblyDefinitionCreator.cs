using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.AssemblyDefinition_Tools
{
    public class AssemblyDefinitionCreator : EditorWindow
    {
        private string rootDirectory = "Assets";
        private string[] assemblyReferences = { "UnityEngine", "UnityEditor" };

        [MenuItem("Tools/Assembly Definition/Creator")]
        public static void ShowWindow()
        {
            GetWindow<AssemblyDefinitionCreator>("Assembly Definition Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Assembly Definition Creator", EditorStyles.boldLabel);

            rootDirectory = EditorGUILayout.TextField("Root Directory", rootDirectory);

            EditorGUILayout.LabelField("Assembly References (comma-separated):");
            string referencesInput = EditorGUILayout.TextField(string.Join(",", assemblyReferences));
            assemblyReferences = referencesInput.Split(',');

            if (GUILayout.Button("Generate Assembly Definitions"))
            {
                GenerateAssemblyDefinitions();
            }
        }

        private void GenerateAssemblyDefinitions()
        {
            if (!Directory.Exists(rootDirectory))
            {
                Debug.LogError($"Directory '{rootDirectory}' does not exist.");
                return;
            }

            string[] directories = Directory.GetDirectories(rootDirectory, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in directories)
            {
                string dirName = Path.GetFileName(dir);
                string asmdefPath = Path.Combine(dir, $"{dirName}.asmdef");

                if (File.Exists(asmdefPath))
                {
                    Debug.LogWarning($"Assembly Definition already exists in '{dir}'. Skipping...");
                    continue;
                }

                AssemblyDefinitionData asmdefData = new AssemblyDefinitionData
                {
                    name = dirName,
                    references = assemblyReferences
                };

                string asmdefJson = JsonUtility.ToJson(asmdefData, true);
                File.WriteAllText(asmdefPath, asmdefJson);

                AssetDatabase.Refresh();
                Debug.Log($"Created Assembly Definition: {asmdefPath}");
            }
        }
        
        private struct AssemblyDefinitionData
        {
            public string name;
            public string[] references;
        }
    }
}