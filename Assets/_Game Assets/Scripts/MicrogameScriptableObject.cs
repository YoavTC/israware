using System;
using NaughtyAttributes;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    [CreateAssetMenu(menuName = "New Microgame Asset", fileName = "Microgame Asset")]
    public class MicrogameScriptableObject : ScriptableObject
    {
        [Header("Meta")]
        public string id;
        public MicrogameFinishType finishType;

        public int positiveFeedbacksToWin;
        public int negativeFeedbacksToLose;
        public float maxMicrogameTime;

        public MicrogameSettingsStruct GetSettings()
        {
            return new MicrogameSettingsStruct(positiveFeedbacksToWin, negativeFeedbacksToLose, maxMicrogameTime);
        }
    }

    [Serializable]
    public struct MicrogameSettingsStruct
    {
        public int positiveFeedbacksToWin;
        public int negativeFeedbacksToLose;
        public float maxMicrogameTime;

        public MicrogameSettingsStruct(int positiveFeedbacksToWin, int negativeFeedbacksToLose, float maxMicrogameTime)
        {
            this.positiveFeedbacksToWin = positiveFeedbacksToWin;
            this.negativeFeedbacksToLose = negativeFeedbacksToLose;
            this.maxMicrogameTime = maxMicrogameTime;
        }
    }

    public enum MicrogameFinishType
    {
        MANUAL,
        FAIL, 
        TIME,
    }
}