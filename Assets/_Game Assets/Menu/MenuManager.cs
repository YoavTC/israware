using System.Linq;
using _Game_Assets.Scripts;
using EditorAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game_Assets.Menu
{
    public class MenuManager : MonoBehaviour
    {
        private StateMachine stateMachine;
        
        private void Start()
        {
            var asyncSceneLoadOperation = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
            asyncSceneLoadOperation.completed += (operation) =>
            {
                Debug.Log("Finished loading Main scene");
                stateMachine = FindObjectsByType<StateMachine>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();
            };
        }

        [Button]
        public void OnPlayButtonPress()
        {
            stateMachine.ChangeState(State.START);
            
            SceneManager.UnloadSceneAsync("Menu");
        }
    }
}
