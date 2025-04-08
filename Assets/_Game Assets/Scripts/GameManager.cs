using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game_Assets.Scripts.ScreenHandlers;
using AYellowpaper.SerializedCollections;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Game_Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Managers")]
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

        [Header("Events")] 
        [SerializeField] private UnityEvent<bool> finishedMicrogameUnityEvent;
        [SerializeField] private UnityEvent<MicrogameScriptableObject> loadedMicrogameUnityEvent;

        // Microgames
        private List<MicrogameScriptableObject> microgames;
        public MicrogameScriptableObject CurrentMicrogame { get; private set; }
        
        private void Start()
        {
            InitializeGameManager();
            StartCoroutine(LoadMicrogame());

            lastMicrogameResult = false;
            health = 5;
            score = 0;
        }
        
        private void InitializeGameManager()
        {
            DontDestroyOnLoad(this);
            var loadedMicrogames = Resources.LoadAll<MicrogameScriptableObject>($"Microgames/")
                .ToList();
            
            // Filter out microgames that are not marked as playable if at least one is marked with "~"
            if (loadedMicrogames.Any(microgame => microgame.name.Contains("~")))
            {
                loadedMicrogames = loadedMicrogames.Where(microgame => microgame.name.Contains("~")).ToList();
            }
            
            microgames = loadedMicrogames;
        }
        
        private IEnumerator LoadMicrogame()
        {
            // Get random microgame
            MicrogameScriptableObject microgame = microgames[Random.Range(0, microgames.Count)];;
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
            
            loadedMicrogameUnityEvent?.Invoke(microgame);
        }

        public void OnTimerFinished(bool win)
        {
            StartCoroutine(UnloadMicrogame(win));
        }

        public IEnumerator UnloadMicrogame(bool win)
        {
            if (!gameActive) yield break;
            
            finishedMicrogameUnityEvent?.Invoke(win);

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