using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
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

        [SerializeField] private float finishDelay;
        
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
        }
        
        public void Feedback(bool positive)
        {
            Debug.Log("Received feedback");
            if (positive) positiveFeedbacksCount++;
            else negativeFeedbacksCount++;

            if (microgameSettings.positiveFeedbacksToWin > 0 && positiveFeedbacksCount >= microgameSettings.positiveFeedbacksToWin)
            {
                StartCoroutine(Finish(true));
            }

            if (microgameSettings.negativeFeedbacksToLose > 0 && negativeFeedbacksCount >= microgameSettings.negativeFeedbacksToLose)
            {
                StartCoroutine(Finish(false));
            }
        }
        
        private IEnumerator Finish(bool win = false)
        {
            if (win) yield return new WaitForSeconds(finishDelay);
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
        
        private void Update()
        {
            if (gameManager == null && Input.GetKeyDown(KeyCode.R))
            {
                EditorSceneManager.LoadSceneInPlayMode(SceneManager.GetActiveScene().path, new LoadSceneParameters(LoadSceneMode.Single));
            }
        }
        #endif
    }
}