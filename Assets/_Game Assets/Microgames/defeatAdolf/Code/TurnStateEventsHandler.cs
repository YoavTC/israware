using System.Collections.Generic;
using _Game_Assets.Microgames.defeatAdolf.Code.Enums;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.defeatAdolf.Code
{
    public class TurnStateEventsHandler : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent<TurnState> stateChangedUnityEvent;
        [SerializeField] private SerializedDictionary<string, UnityEvent> animationEventsDictionary;
        
        private void InvokeEvent(string eventName)
        {
            if (animationEventsDictionary.TryGetValue(eventName, out UnityEvent animationEvent))
            {
                animationEvent?.Invoke();
            } else Debug.LogWarning($"Event '{eventName}' not found in the dictionary.");
        }
        
        // Generic events
        [FoldoutGroup("Generic Events", nameof(choosingStateUnityEvent), nameof(performingStateUnityEvent))]
        [SerializeField] private Void genericEventsGroupHolder;
        [SerializeField, HideProperty] private UnityEvent<TurnState> choosingStateUnityEvent;
        [SerializeField, HideProperty] private UnityEvent<TurnState> performingStateUnityEvent;
        
        // Specific events
        [FoldoutGroup("Specific Events",
            nameof(introStateUnityEvent),
            nameof(playerChoosingAttackStateUnityEvent),
            nameof(playerPerformingAttackStateUnityEvent),
            nameof(enemyChoosingAttackStateUnityEvent),
            nameof(enemyPerformingAttackStateUnityEvent),
            nameof(gameOverStateUnityEvent)
        )]
        [SerializeField] private Void specificEventsGroupHolder;
        [SerializeField, HideProperty] private UnityEvent<TurnState> introStateUnityEvent;
        [SerializeField, HideProperty] private UnityEvent<TurnState> playerChoosingAttackStateUnityEvent;
        [SerializeField, HideProperty] private UnityEvent<TurnState> playerPerformingAttackStateUnityEvent;
        [SerializeField, HideProperty] private UnityEvent<TurnState> enemyChoosingAttackStateUnityEvent;
        [SerializeField, HideProperty] private UnityEvent<TurnState> enemyPerformingAttackStateUnityEvent;
        [SerializeField, HideProperty] private UnityEvent<TurnState> gameOverStateUnityEvent;
        
        private Dictionary<TurnState, List<UnityEvent<TurnState>>> stateChangedEventsDictionary = new();

        private void Start()
        {
            RegisterStateChangedEvents();
        }

        private void RegisterStateChangedEvents()
        {
            // Specific events
            AddEvent(TurnState.INTRO, introStateUnityEvent);
            AddEvent(TurnState.PLAYER_CHOOSING_ATTACK, playerChoosingAttackStateUnityEvent);
            AddEvent(TurnState.PLAYER_PERFORMING_ATTACK, playerPerformingAttackStateUnityEvent);
            AddEvent(TurnState.ENEMY_CHOOSING_ATTACK, enemyChoosingAttackStateUnityEvent);
            AddEvent(TurnState.ENEMY_PERFORMING_ATTACK, enemyPerformingAttackStateUnityEvent);
            AddEvent(TurnState.GAME_OVER, gameOverStateUnityEvent);
            
            // Generic events
            AddEvent(TurnState.PLAYER_CHOOSING_ATTACK, choosingStateUnityEvent);
            AddEvent(TurnState.ENEMY_CHOOSING_ATTACK, choosingStateUnityEvent);
            AddEvent(TurnState.PLAYER_PERFORMING_ATTACK, performingStateUnityEvent);
            AddEvent(TurnState.ENEMY_PERFORMING_ATTACK, performingStateUnityEvent);
        }

        // Utility method to add events to the dictionary
        private void AddEvent(TurnState state, UnityEvent<TurnState> unityEvent)
        {
            if (!stateChangedEventsDictionary.ContainsKey(state))
                stateChangedEventsDictionary[state] = new List<UnityEvent<TurnState>>();

            stateChangedEventsDictionary[state].Add(unityEvent);
        }

        public void UpdateState(TurnState newState)
        {
            // Invoke all events registered for the state
            if (stateChangedEventsDictionary.TryGetValue(newState, out var eventList))
            {
                foreach (var unityEvent in eventList)
                {
                    unityEvent?.Invoke(newState);
                }
            }

            stateChangedUnityEvent?.Invoke(newState);
        }
    }
}