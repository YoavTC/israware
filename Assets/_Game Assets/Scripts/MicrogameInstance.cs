using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game_Assets.Scripts
{
    public class MicrogameInstance : MonoBehaviour
    {
        private GameManager gameManager;
        [SerializeField, ReadOnly] private MicrogameScriptableObject microgame;

        [SerializeField, ReadOnly] private MicrogameSettingsStruct microgameSettings;
        private int negativeFeedbacksCount;
        private int positiveFeedbacksCount;
        
        private void Start()
        {
            gameManager = GameManager.Instance;
            
            if (gameManager != null)
            {
                Debug.Log("Manager is not null using it..");
                microgame = gameManager.CurrentMicrogame;
            } else LoadMicrogameScriptableObject();
            
            Cursor.visible = !microgameSettings.hideCursor;
            microgameSettings = microgame.GetSettings();
            
            if (Timer.Instance != null) Timer.Instance?.StartTimer(microgameSettings.maxMicrogameTime, microgameSettings.winAtTimerFinish);
        }
        
        public void Feedback(bool positive)
        {
            Debug.Log("Received feedback");
            if (positive) positiveFeedbacksCount++;
            else negativeFeedbacksCount++;

            if (microgameSettings.positiveFeedbacksToWin > 0 && positiveFeedbacksCount >= microgameSettings.positiveFeedbacksToWin)
            {
                Finish(true);
            }

            if (microgameSettings.negativeFeedbacksToLose > 0 && negativeFeedbacksCount >= microgameSettings.negativeFeedbacksToLose)
            {
                Finish(false);
            }
        }

        private void Finish(bool win = false)
        {
            if (gameManager != null)
            {
                StartCoroutine(gameManager.OnMicrogameFinished(win));
            }
        }

        private void LoadMicrogameScriptableObject()
        {
            microgame = Resources.Load<MicrogameScriptableObject>($"Microgames/{SceneManager.GetActiveScene().name}");
            microgameSettings = microgame.GetSettings();
        }
        
        #if UNITY_EDITOR
        [Button]
        private void ValidateSettings()
        {
            LoadMicrogameScriptableObject();
        }
        #endif
    }
}