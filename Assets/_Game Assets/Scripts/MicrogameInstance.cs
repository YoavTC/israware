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
        
        private IEnumerator Start()
        {
            gameManager = GameManager.Instance;
            microgame = gameManager.CurrentMicrogame;

            microgameSettings = microgame.GetSettings();

            yield return new WaitForSeconds(microgameSettings.maxMicrogameTime);
            Feedback(false, true);
        }

        public void Feedback(bool positive) => Feedback(positive, false);
        private void Feedback(bool positive, bool force)
        {
            if (force)
            {
                Finish(positive);
                return;
            }
            
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
            gameManager.FinishMicrogame(win);
        }
    }
}