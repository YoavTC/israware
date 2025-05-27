using System.Linq;
using _Game_Assets.Scripts.Definitions;
using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class MicrogameProvider : MonoBehaviour
    {
        [SerializeField] private MicrogameScriptableObject[] microgames;

        [SerializeField, HideInEditMode] private MicrogameScriptableObject currentMicrogame;
        [SerializeField, HideInEditMode] private MicrogameScriptableObject nextMicrogame;
        
        private void Awake()
        {
            RetrieveMicrogames();
            AdvanceQueue();
        }

        private void RetrieveMicrogames()
        {
            DontDestroyOnLoad(this);
            var loadedMicrogames = Resources.LoadAll<MicrogameScriptableObject>($"Microgames/").ToArray();
            
            // Filter out microgames that are not marked as playable if at least one is marked with "~"
            if (loadedMicrogames.Any(microgame => microgame.name.Contains("~")))
            {
                loadedMicrogames = loadedMicrogames.Where(microgame => microgame.name.Contains("~")).ToArray();
            }
            
            microgames = loadedMicrogames;
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
            
            // TODO: Replace with actual logic to determine the next microgame
            nextMicrogame = microgames[Random.Range(0, microgames.Length)];
        }
    }
}
