using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game_Assets.Scripts.Definitions;
using _Game_Assets.Scripts.Screen_Handlers;
using EditorAttributes;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace _Game_Assets.Scripts
{
    public class StateMachine : Singleton<StateMachine>
    {
        [Header("Managers")] 
        [SerializeField] private GameObject screensParent;
        [SerializeField] private MicrogameProvider microgameProvider;
        [SerializeField] private Timer timer;
        public Timer Timer => timer;
        
        [Header("Screens")]
        [SerializeField] private List<BaseScreen> screens;

        [Header("Microgame callbacks Listeners")] 
        [SerializeField] private MonoBehaviour[] microgameCallbacksListeners;
        
        [SerializeField] private State currentState;
        private bool isDead;
        private AsyncOperation gameSceneLoadOperation;
        
        public MicrogameScriptableObject CurrentMicrogame { private set; get; }
        
        [SerializeField] private bool lastGameResult;
        
        private void Start()
        {
            gameSceneLoadOperation = null;
            isDead = false;
            PlayerPrefs.SetInt("_SCORE", 0);
            screensParent.SetActive(false);
            PrepareGame();
        }

        [Button]
        public void OnMicrogameFinished(bool won)
        {
            lastGameResult = won;
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
            SceneManager.UnloadSceneAsync(CurrentMicrogame.id);
            
            ChangeState(State.STATUS);
        }

        [Button]
        public void ChangeState(State state)
        {
            Debug.Log($"Received new state [{state}]");

            // If already dead or same state, ignore
            if (currentState == state || currentState == State.DEATH) return;

            switch (state)
            {
                case State.START:
                    ChangeState(State.GAME);
                    screensParent.SetActive(true);
                    break;
                case State.GAME:
                    StartCoroutine(LoadGame());
                    break;
                case State.STATUS:
                    NotifyMicrogameCallbackListeners(lastGameResult);
                    PrepareGame();
                    ShowStatus();
                    break;
                    case State.DEATH:
                        // Death();
                        isDead = true;
                        break;
            }

            Debug.Log($"Changing current state to [{state}]");
            currentState = state;
        }

        private void PrepareGame()
        {
            MicrogameScriptableObject microgame = microgameProvider.GetMicrogame();
            
            // Load the scene asynchronously
            var loadSceneAsync = SceneManager.LoadSceneAsync(microgame.id, LoadSceneMode.Additive);
            if (loadSceneAsync != null)
            {
                // Disable automatic scene switching
                loadSceneAsync.allowSceneActivation = false;
                
                gameSceneLoadOperation = loadSceneAsync;
                CurrentMicrogame = microgame;
            }
        }

        private IEnumerator LoadGame()
        {
            if (isDead)
            {
                Death();
                yield break;
            }

            Debug.Log("Loading game scene...");
            yield return new WaitUntil(() => gameSceneLoadOperation.progress >= 0.9f);

            
            // Allow the scene switch
            gameSceneLoadOperation.allowSceneActivation = true;

            Debug.Log("Game scene loaded");
            yield return null; // Wait for the next frame to ensure that the scene load operation is complete

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(CurrentMicrogame.id));
            
            // Hide screens
            screens.ForEach(screen => screen.Hide());

            gameSceneLoadOperation = null;
            
            NotifyMicrogameCallbackListeners(CurrentMicrogame);
        }
        
        private void ShowStatus()
        {
            Debug.Log("Showing status screen");
            // Hide screens
            screens.ForEach(screen => screen.Show(lastGameResult));
        }

        private void Death()
        {
            StopAllCoroutines();
            Debug.Log("Death state reached, showing death screen");
            SceneManager.LoadScene("Death", LoadSceneMode.Single);
        }
        
        private void NotifyMicrogameCallbackListeners(Object parameter)
        {
            foreach (var listener in microgameCallbacksListeners.OfType<IMicrogameCallbacksListener>())
            {
                listener.ReceiveMicrogameCallback(parameter);
            }
        }
    }
}