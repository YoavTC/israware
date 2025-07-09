using System.Collections;
using _Game_Assets.Scripts.Definitions;
using EditorAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game_Assets.Scripts
{
    public enum State
    {
        GAME,
        STATUS,
        DEATH,
    }
    
    public class StateMachine : MonoBehaviour
    {
        [Header("Managers")] 
        [SerializeField] private MicrogameProvider microgameProvider;
        [SerializeField] private StatusScreen statusScreen;
        
        private AsyncOperation gameSceneLoadOperation;
        private bool lastGameResult;
        
        [SerializeField] private State currentState;

        private void Start()
        {
            gameSceneLoadOperation = null;
            
            PrepareGame();
            ChangeState(State.GAME);
        }

        [Button]
        public void OnMicrogameFinished(bool won)
        {
            lastGameResult = won;
            ChangeState(State.STATUS);
        }

        [Button]
        public void ChangeState(State state)
        {
            Debug.Log($"Received new state [{state}]");
            
            switch (state)
            {
                case State.GAME:
                    StartCoroutine(LoadGame());
                    break;
                case State.STATUS:
                    PrepareGame();
                    ShowStatus();
                    break;
                case State.DEATH:
                    Death();
                    break;
            }
            
            Debug.Log($"Changing current state to [{state}]");
            currentState = state;
        }

        private void PrepareGame()
        {
            MicrogameScriptableObject microgame = microgameProvider.GetMicrogame();
            
            // Load the scene asynchronously
            var loadSceneAsync = SceneManager.LoadSceneAsync(microgame.id);
            if (loadSceneAsync != null)
            {
                // Disable automatic scene switching
                loadSceneAsync.allowSceneActivation = false;
                gameSceneLoadOperation = loadSceneAsync;
            }
        }

        private IEnumerator LoadGame()
        {
            Debug.Log("Loading game scene...");
            yield return new WaitUntil(() => gameSceneLoadOperation.progress >= 0.9f);
            
            // Allow the scene switch
            gameSceneLoadOperation.allowSceneActivation = true;

            Debug.Log("Game scene loaded");
            yield return null; // Wait for the next frame to ensure that the scene load operation is complete

            statusScreen.HideStatus();
            gameSceneLoadOperation = null;
        }
        
        private void ShowStatus()
        {
            Debug.Log("Showing status screen");
            statusScreen.ShowStatus(lastGameResult);
        }

        private void Death()
        {
            Debug.Log("Death state reached, showing death screen");
        }
    }
}