using System.Collections;
using _Game_Assets.Scripts.Definitions;
using EditorAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace _Game_Assets.Scripts
{
    public class MicrogameInstance : MonoBehaviour
    {
        [Header("Microgame Settings")]
        [SerializeField, ReadOnly] private MicrogameScriptableObject microgame;
        [SerializeField, DisableInEditMode, InlineButton(nameof(PositiveFeedback), "+", 50f)] private int positiveFeedbacksCount;
        [SerializeField, DisableInEditMode, InlineButton(nameof(NegativeFeedback), "+", 50f)] private int negativeFeedbacksCount;
        
        [Header("Delay Settings")]
        [SerializeField] private float winFinishDelay;
        [SerializeField] private float LoseFinishDelay;
        
        private StateMachine stateMachine;
        
        private void Start()
        {
            stateMachine = StateMachine.Instance;
            
            microgame = stateMachine != null ? 
                stateMachine.CurrentMicrogame : GetMicrogame();
            
            Cursor.visible = !microgame.hideCursor;
        }

        private void PositiveFeedback() => Feedback(true);
        private void NegativeFeedback() => Feedback(false);
        
        public void Feedback(bool positive)
        {
            Debug.Log($"Received {(positive ? "positive" : "negative")} feedback");
            if (positive) positiveFeedbacksCount++;
            else negativeFeedbacksCount++;

            if (stateMachine == null) return;
            if (microgame.positiveFeedbacksToWin > 0 && positiveFeedbacksCount >= microgame.positiveFeedbacksToWin)
            {
                StartCoroutine(Finish(true));
            }

            if (microgame.negativeFeedbacksToLose > 0 && negativeFeedbacksCount >= microgame.negativeFeedbacksToLose)
            {
                StartCoroutine(Finish(false));
            }
        }

        private IEnumerator Finish(bool win)
        {
            stateMachine.Timer?.DisableTimer();

            yield return new WaitForSeconds(win ? winFinishDelay : LoseFinishDelay);
            stateMachine?.OnMicrogameFinished(win);
        }
        
        private MicrogameScriptableObject GetMicrogame()
        {
            return Resources.Load<MicrogameScriptableObject>($"Microgames/{SceneManager.GetActiveScene().name}");
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (microgame == null)
            {
                microgame = GetMicrogame();
            }
        }

        private void Update()
        {
            if (!stateMachine && Input.GetKeyDown(KeyCode.R))
            {
                EditorSceneManager.LoadSceneInPlayMode(SceneManager.GetActiveScene().path, new LoadSceneParameters(LoadSceneMode.Single));
            }
        }
        #endif
    }
}