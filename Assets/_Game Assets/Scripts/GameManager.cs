using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using External_Packages.MonoBehaviour_Extensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace _Game_Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private bool gameActive;
        [SerializeField] private List<MicrogameScriptableObject> microgames;
        public MicrogameScriptableObject CurrentMicrogame { get; private set; }

        [Header("Components")] 
        [SerializeField] private TransitionDoor transitionDoor;
        [SerializeField] private ResultScreenHandler resultScreenHandler;

        private void InitializeGameManager()
        {
            DontDestroyOnLoad(this);
            microgames = Resources.LoadAll<MicrogameScriptableObject>("Microgames/").ToList();
        }
        
        private void Start()
        {
            InitializeGameManager();

            StartCoroutine(TransitionMicrogame());
        }
        
        public void FinishedMicrogame(bool win)
        {
            resultScreenHandler.SetLastMicrogameResult(win);
            StartCoroutine(TransitionMicrogame());
            
            Timer.Instance.DisableTimer();
        }

        private IEnumerator TransitionMicrogame()
        {
            // Get random microgame
            MicrogameScriptableObject microgame = GetRandomMicrogame();
            CurrentMicrogame = microgame;
            
            // Start loading the microgame scene
            var loadSceneAsync = SceneManager.LoadSceneAsync(microgame.id);
            if (loadSceneAsync == null) yield break;
            loadSceneAsync.allowSceneActivation = false;
            
            // Close and open the transition door
            yield return StartCoroutine(transitionDoor.Toggle(() =>
            {
                // Enable the between-screen
                resultScreenHandler.ToggleVisibility(true);
            }));

            // Animate the between-screen
            yield return StartCoroutine(resultScreenHandler.Animate());
 
            // When the screen is finished animating, wait until the scene is fully loaded if it already isn't
            yield return new WaitUntil(() => loadSceneAsync.progress >= 0.9f);
            
            // Close and open the transition door
            yield return StartCoroutine(transitionDoor.Toggle(() =>
            {
                // Disable the between-screen & allow the scene to be loaded
                resultScreenHandler.ToggleVisibility(false);
                loadSceneAsync.allowSceneActivation = true;
            }));
        }
        
        private MicrogameScriptableObject GetRandomMicrogame()
        {
            return microgames[Random.Range(0, microgames.Count)];
        }
    }
}
