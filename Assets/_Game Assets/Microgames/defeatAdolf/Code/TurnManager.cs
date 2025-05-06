using System;
using System.Collections;
using System.Collections.Generic;
using _Game_Assets.Microgames.defeatAdolf.Code.Enums;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Playables;

namespace _Game_Assets.Microgames.defeatAdolf.Code
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private TurnStateEventsHandler turnStateEventsHandler;
        [SerializeField] private PlayableDirector introDirector;
        [SerializeField] private Animator actionsAnimator;

        private ActionType chosenAction;

        [Header("State Machine")]
        [SerializeField] private TurnState currentState;
        [SerializeField] private bool waitingForInput;
        public TurnState CurrentState => currentState;
        
        // Dictionary to hold the factory instances of state coroutines
        // to allow reuse of the same coroutines
        private Dictionary<TurnState, Func<IEnumerator>> statesCoroutines;
        
        #region Trun State Flow Handling
        private void Start()
        {
            statesCoroutines = new Dictionary<TurnState, Func<IEnumerator>>
            {
                { TurnState.INTRO, IntroStateCoroutine },
                { TurnState.PLAYER_CHOOSING_ATTACK, PlayerChoosingAttackCoroutine },
                { TurnState.PLAYER_PERFORMING_ATTACK, PlayerPerformingAttackCoroutine },
                { TurnState.ENEMY_CHOOSING_ATTACK, EnemyChoosingAttackCoroutine },
                { TurnState.ENEMY_PERFORMING_ATTACK, EnemyPerformingAttackCoroutine },
                { TurnState.GAME_OVER, GameOverCoroutine }
            };

            UpdateState(TurnState.INTRO);
        }

        private void UpdateState(TurnState newState)
        {
            StartCoroutine(statesCoroutines[newState]());

            currentState = newState;
            turnStateEventsHandler.UpdateState(newState);
        }

        // Called by external systems to let the state
        // machine know that the current state has finished
        public void FinishState()
        {
            Debug.Log("Finished state: " + currentState);
            waitingForInput = false;
        }

        private IEnumerator WaitForState(TurnState turnState)
        {
            waitingForInput = true;
            yield return new WaitUntil(() => !waitingForInput);
        }
        #endregion
        
        public void ActionChosen(ActionType actionType)
        {
            chosenAction = actionType;
        }

        #region Turn state coroutines
        private IEnumerator IntroStateCoroutine()
        {
            introDirector.Play();
            yield return new WaitForSeconds((float)introDirector.duration);

            UpdateState(TurnState.PLAYER_CHOOSING_ATTACK);
        }

        private IEnumerator PlayerChoosingAttackCoroutine()
        {
            // Wait for player to choose an action
            yield return WaitForState(TurnState.PLAYER_CHOOSING_ATTACK);

            UpdateState(TurnState.PLAYER_PERFORMING_ATTACK);
        }

        private IEnumerator PlayerPerformingAttackCoroutine()
        {
            // Animate the action & notify affected entities
            actionsAnimator.SetTrigger(chosenAction.ToString("G"));
            yield return WaitForState(TurnState.PLAYER_PERFORMING_ATTACK);

            UpdateState(TurnState.ENEMY_CHOOSING_ATTACK);
        }

        private IEnumerator EnemyChoosingAttackCoroutine()
        {
            // Enemy AI logic to choose an action
            yield return WaitForState(TurnState.ENEMY_CHOOSING_ATTACK);

            UpdateState(TurnState.ENEMY_PERFORMING_ATTACK);
        }

        private IEnumerator EnemyPerformingAttackCoroutine()
        {
            // Animate the action & notify affected entities
            yield return WaitForState(TurnState.ENEMY_PERFORMING_ATTACK);

            UpdateState(TurnState.PLAYER_CHOOSING_ATTACK);
        }

        private IEnumerator GameOverCoroutine()
        {
            // Game over screen
            yield return WaitForState(TurnState.GAME_OVER);

            UpdateState(TurnState.INTRO);
        }
        #endregion
    }
}