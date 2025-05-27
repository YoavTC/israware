using System.Collections;
using System.Linq;
using _Game_Assets.Scripts.Definitions;
using AYellowpaper.SerializedCollections;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace _Game_Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Managers")] 
        [SerializeField] private MicrogameProvider microgameProvider;
        [SerializeField] private Timer timer;
        public Timer Timer => timer;
        
        [Header("Screens")] 
        [SerializeField] private float defaultShowScreenDuration;
        [SerializeField] private SerializedDictionary<ScreenType, ScreenHandlerBase> screenHandlersDictionary;

        [Header("Game Variables")]
        [SerializeField] private int health;
        [SerializeField] private int score;
        [SerializeField] private bool gameActive;
        [SerializeField] private bool lastMicrogameResult;

        [Header("Microgame callbacks Listeners")] 
        [SerializeField] private MonoBehaviour[] microgameCallbacksListeners;

        // Microgames
        public MicrogameScriptableObject CurrentMicrogame { get; private set; }
        
        private void Start()
        {
            StartCoroutine(LoadMicrogame());

            lastMicrogameResult = false;
            health = 5;
            score = 0;
        }
        
        private IEnumerator LoadMicrogame()
        {
            // Get random microgame
            MicrogameScriptableObject microgame = microgameProvider.GetMicrogame();
            CurrentMicrogame = microgame;
            
            // Start loading the microgame scene
            var loadSceneAsync = SceneManager.LoadSceneAsync(microgame.id);
            if (loadSceneAsync == null) yield break;
            loadSceneAsync.allowSceneActivation = false;
            
            // Hide the status overlay when the scene finishes loading
            loadSceneAsync.completed += operation =>
            {
                HideScreen(ScreenType.HEALTH);
                HideScreen(ScreenType.STATUS);
            }; 

            StartCoroutine(ShowScreen(ScreenType.STATUS, -1f));
            
            // When the screen is finished animating, wait until the scene is fully loaded
            yield return new WaitUntil(() => loadSceneAsync.progress >= 0.9f);
            yield return new WaitForSeconds(defaultShowScreenDuration);
            
            // Activate the scene
            gameActive = true;
            loadSceneAsync.allowSceneActivation = true;
            
            // microgameLoadedUnityEvent?.Invoke(microgame);
            NotifyMicrogameCallbackListeners(microgame);
        }

        public void OnTimerFinished(bool win)
        {
            StartCoroutine(UnloadMicrogame(win));
        }

        public IEnumerator UnloadMicrogame(bool win)
        {
            if (!gameActive) yield break;
            
            // microgameFinishedUnityEvent?.Invoke(win);
            NotifyMicrogameCallbackListeners(win);

            gameActive = false;
            lastMicrogameResult = win;
            
            if (win) score++;
            bool dead = UpdateHealth(win);

            // Show the feedback overlay
            StartCoroutine(ShowScreen(ScreenType.HEALTH, -1f));
            yield return StartCoroutine(ShowScreen(win ? ScreenType.POSITIVE : ScreenType.NEGATIVE, defaultShowScreenDuration));
            
            StartCoroutine(dead ? ShowScreen(ScreenType.GAME_OVER, -1f) : 
                LoadMicrogame());
        }

        private bool UpdateHealth(bool win)
        {
            health += win ? 0 : -1;
            return health <= 0;
        }

        private IEnumerator ShowScreen(ScreenType screenType, float duration)
        {
            yield return StartCoroutine(screenHandlersDictionary[screenType].Show(duration, lastMicrogameResult, health, score));
        }

        private void HideScreen(ScreenType screenType)
        {
            screenHandlersDictionary[screenType]?.Hide();
        }
        
        private void NotifyMicrogameCallbackListeners(Object parameter)
        {
            foreach (var listener in microgameCallbacksListeners.OfType<IMicrogameCallbacksListener>())
            {
                listener.ReceiveMicrogameCallback(parameter);
            }
        }

        #if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                StartCoroutine(UnloadMicrogame(true));
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                StartCoroutine(UnloadMicrogame(false));
            }
        }
        #endif
    }
}