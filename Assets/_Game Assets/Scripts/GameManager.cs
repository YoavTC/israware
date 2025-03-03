using System.Collections.Generic;
using External_Packages.MonoBehaviour_Extensions;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        private List<MicrogameScriptableObject> microgames;

        private void LoadMicrogames()
        {
            // Resources.LoadAll<TextAsset>("")
        }
        
        public void StartMicrogame()
        {
            
        }

        public void FinishMicrogame()
        {
            
        }
    }
}
