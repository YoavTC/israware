using System.Linq;
using _Game_Assets.Scripts.Definitions;
using EditorAttributes;
using External_Packages.Extensions;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class MicrogameProvider : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool loadMicrogamesFromResources;
        
        [Header("Microgames")]
        [SerializeField] private MicrogameScriptableObject[] allMicrogames;
        [SerializeField] private string[] microgamesIDs;

        [Header("Debug")]
        [SerializeField, DisableInEditMode] private int currentMicrogameIndex;
        [Space]
        [SerializeField, ReadOnly] private MicrogameScriptableObject currentMicrogame;
        [SerializeField, ReadOnly] private MicrogameScriptableObject nextMicrogame;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (loadMicrogamesFromResources) 
                LoadMicrogamesFiles();
            
            SortMicrogames();
            InitializeQueue(); // Initialize the queue properly to avoid nulls
        }

        private void LoadMicrogamesFiles()
        {
            var loadedMicrogames = Resources.LoadAll<MicrogameScriptableObject>($"Microgames/").ToArray();
            
            // If any microgame names contain "~", filter the ones that don't out
            if (loadedMicrogames.Any(microgame => microgame.name.Contains("~")))
            {
                loadedMicrogames = loadedMicrogames.Where(microgame => microgame.name.Contains("~")).ToArray();
            }
            
            allMicrogames = loadedMicrogames;
        }
        
        private void SortMicrogames()
        {
            // Randomize the microgames array
            allMicrogames = allMicrogames.Shuffle().ToArray();
            microgamesIDs = allMicrogames.Select(micro => micro.id).ToArray();
        }
        
        private void InitializeQueue()
        {
            currentMicrogameIndex = -1;
            nextMicrogame = allMicrogames[0]; // Preload the first microgame
            AdvanceQueue(); // Set the first currentMicrogame
        }

        // Called from the GameManager to return the next microgame
        public MicrogameScriptableObject GetMicrogame()
        {
            AdvanceQueue();
            return currentMicrogame;
        }
        
        private void AdvanceQueue()
        {
            currentMicrogame = nextMicrogame;
            currentMicrogameIndex++;

            // Bounds check for overflow
            if (currentMicrogameIndex >= 0 && currentMicrogameIndex < microgamesIDs.Length)
            {
                string nextId = microgamesIDs[currentMicrogameIndex];
                nextMicrogame = allMicrogames.FirstOrDefault(mg => mg.id == nextId);
            }
            else
            {
                Debug.Log("No more microgames available, resetting queue.");
                nextMicrogame = null; // Handle end of list gracefully
            }
        }
    }
}