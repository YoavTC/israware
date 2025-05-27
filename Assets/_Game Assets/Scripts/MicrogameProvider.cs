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
        [SerializeField] private string[] bossLevelMicrogamesIDs;

        [Header("Debug")]
        [SerializeField, DisableInEditMode] private int currentMicrogameIndex;
        [SerializeField, DisableInEditMode] private int currentMicrogameSubgroupIndex;
        [SerializeField, DisableInEditMode] private int microgamesSubgroupSize;
        [Space]
        [SerializeField, ReadOnly] private MicrogameScriptableObject currentMicrogame;
        [SerializeField, ReadOnly] private MicrogameScriptableObject nextMicrogame;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (loadMicrogamesFromResources) 
                LoadMicrogamesFiles();
            
            SortMicrogames(); // Sort microgames by difficulty and split into boss and regular levels
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
            // Sort microgames by difficulty
            allMicrogames = allMicrogames.OrderBy(microgame => microgame.difficulty).ToArray();

            // Split microgames into boss levels and regular levels
            bossLevelMicrogamesIDs = allMicrogames
                .Where(microgame => microgame.isBossLevel)
                .Select(microgame => microgame.id)
                .ToArray();
            
            microgamesIDs = allMicrogames
                .Where(microgame => !microgame.isBossLevel)
                .Select(microgame => microgame.id)
                .ToArray();
        }
        
        private void InitializeQueue()
        {
            currentMicrogameIndex = -1;
            currentMicrogameSubgroupIndex = -1;
            nextMicrogame = allMicrogames[0]; // Preload the first microgame
            AdvanceQueue(); // Set the first currentMicrogame
        }

        // Called from the GameManager to return the next microgame
        public MicrogameScriptableObject GetMicrogame()
        {
            AdvanceQueue();
            return currentMicrogame;
        }

        // Copilot Suggestion: Refactored to avoid repeated ToArray/Where allocations
        private void AdvanceQueue()
        {
            currentMicrogame = nextMicrogame;

            currentMicrogameIndex++;
            currentMicrogameSubgroupIndex++;

            if (currentMicrogameSubgroupIndex >= microgamesSubgroupSize)
            {
                currentMicrogameSubgroupIndex = 0;
                
                string randomBossLevelId = bossLevelMicrogamesIDs.Random();
                nextMicrogame = allMicrogames.FirstOrDefault(microgame => microgame.id == randomBossLevelId);
            }
            else
            {
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
}