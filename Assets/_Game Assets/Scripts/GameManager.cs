using System.Collections;
using System.Collections.Generic;
using System.Linq;
using External_Packages.MonoBehaviour_Extensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game_Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private bool gameActive;
        
        [SerializeField] private List<MicrogameScriptableObject> microgames;
        [SerializeField, ReadOnly] private MicrogameScriptableObject currentMicrogame;
        public MicrogameScriptableObject CurrentMicrogame => currentMicrogame;
        private MicrogameScriptableObject lastMicrogame;

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
            Debug.Log($"Loading [{microgame.id}]");
            
            currentMicrogame = microgame;
            SceneManager.LoadScene(currentMicrogame.id);
        }
        
        public void FinishMicrogame(bool win)
        {
            Debug.Log($"{(win ? "Won" : "Lost")} microgame [{currentMicrogame.id}]");

            lastMicrogame = currentMicrogame;
            currentMicrogame = null;
        }
    }
}
