using System.Collections;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class MicrogameInstance : MonoBehaviour
    {
        private GameManager gameManager;
        private MicrogameScriptableObject microgame;

        private MicrogameSettingsStruct microgameSettings;
        private int negativeFeedbacksCount;
        private int positiveFeedbacksCount;
        
        private void Start()
        {
            gameManager = GameManager.Instance;
            microgame = gameManager.CurrentMicrogame;

            microgameSettings = microgame.GetSettings();

            Timer.Instance.StartTimer(microgameSettings.maxMicrogameTime);
        }
        
        public void Feedback(bool positive)
        {
            if (positive) positiveFeedbacksCount++;
            else negativeFeedbacksCount++;

            if (microgameSettings.positiveFeedbacksToWin <= positiveFeedbacksCount)
            {
                Finish(true);
            }

            if (microgameSettings.negativeFeedbacksToLose <= negativeFeedbacksCount)
            {
                Finish(false);
            }
        }

        private void Finish(bool win = false)
        {
            StartCoroutine(gameManager.FinishedMicrogame(win));
        }
    }
}