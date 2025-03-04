using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _Game_Assets.Scripts.Transition;
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
        [SerializeField, ReadOnly] private MicrogameScriptableObject currentMicrogame;
        public MicrogameScriptableObject CurrentMicrogame => currentMicrogame;
        private MicrogameScriptableObject lastMicrogame;

        [Header("Components")] 
        [SerializeField] private TransitionDoor transitionDoor;

        private void InitializeGameManager()
        {
            DontDestroyOnLoad(this);
            microgames = Resources.LoadAll<MicrogameScriptableObject>("Microgames/").ToList();
        }
        
        private IEnumerator Start()
        {
            InitializeGameManager();
            
            while (gameActive)
            {
                StartMicrogame(GetRandomMicrogame());
                yield return new WaitUntil(() => currentMicrogame == null);
            }
        }

        private MicrogameScriptableObject GetRandomMicrogame()
        {
            if (microgames.Count <= 1) return microgames.FirstOrDefault();

            MicrogameScriptableObject microgame;
            do
            {
                microgame = microgames[Random.Range(0, microgames.Count)];
            } 
            while (microgame == lastMicrogame);

            return microgame;
        }
        
        private void StartMicrogame(MicrogameScriptableObject microgame)
        {
            currentMicrogame = microgame;
            SceneManager.LoadScene(currentMicrogame.id);
        }
        
        public void FinishMicrogame(bool win)
        {
            Debug.Log($"{(win ? "Won" : "Lost")} microgame [{currentMicrogame.id}]");

            StartCoroutine(Transition());
            
            // lastMicrogame = currentMicrogame;
            // currentMicrogame = null;
        }

        private IEnumerator Transition()
        {
            MicrogameScriptableObject microgame = GetRandomMicrogame();
            lastMicrogame = currentMicrogame;
            currentMicrogame = microgame;
            
            var loadSceneAsync = SceneManager.LoadSceneAsync(microgame.id);
            if (loadSceneAsync == null) yield break;
            
            transitionDoor.Close();
            loadSceneAsync.allowSceneActivation = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            yield return new WaitUntil(() => loadSceneAsync.progress >= 0.9f);
            
            stopwatch.Stop();
            float elapsedSeconds = (float) stopwatch.Elapsed.TotalSeconds;
            if (elapsedSeconds < 0.5f) yield return new WaitForSeconds((0.5f - elapsedSeconds) + elapsedSeconds);
            
            loadSceneAsync.allowSceneActivation = true;
            transitionDoor.Open();
            
        }
    }
}
