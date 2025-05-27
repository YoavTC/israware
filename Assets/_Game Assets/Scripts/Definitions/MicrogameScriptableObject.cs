using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Scripts.Definitions
{
    [CreateAssetMenu(menuName = "New Microgame Asset", fileName = "Microgame Asset")]
    public class MicrogameScriptableObject : ScriptableObject
    {
        [Title(nameof(Title), stringInputMode: StringInputMode.Dynamic)]
        [ReadOnly] public string id;
        private string Title => "ID: " + id;
        
        [Header("Prompt")]
        public string ENGLISH_PROMPT;
        public string HEBREW_PROMPT;
        
        [Header("Settings")]
        public bool hideCursor;
        
        public int positiveFeedbacksToWin;
        public int negativeFeedbacksToLose;
        
        public float maxMicrogameTime;
        public bool winAtTimerFinish;
        
        [Header("Meta Information")]
        public bool isBossLevel;
        [Range(0f, 100f)] public float difficulty;
    }
}