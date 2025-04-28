using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityUtils;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.mahsaneiHashmal
{
    public class WordManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform canvas;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        
        [Header("Settings")]
        [SerializeField] private TMP_Text wordAliasPrefab;
        [SerializeField] private float wordSnapDistance;
        [Space]
        [SerializeField] private SerializedDictionary<TMP_Text, string[]> wordAliasesDictionary;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<bool> wordSubmittedUnityEvent;
        
        // Random word display
        private TMP_Text randomWordDisplay;
        private string originalRandomWordValue;
        
        // Runtime instantiated word aliases
        private List<TMP_Text> wordAliases = new List<TMP_Text>();

        // Grabbed word alias
        private Transform grabbedWordAliasTransform;
        private Vector2 grabOffset;
        
        // Word alias display width offset adjustment
        private const float WORD_ALIAS_DISPLAY_WIDTH = 17f;
        private Vector3 wordAliasLabelWidth => new Vector3(0f, WORD_ALIAS_DISPLAY_WIDTH, 0f);

        private void Start()
        {
            // Select random word display and instantiate its aliases
            randomWordDisplay = wordAliasesDictionary.Keys.ToArray()[Random.Range(0, wordAliasesDictionary.Count)];
            PopulateAliases(wordAliasesDictionary[randomWordDisplay].Append(randomWordDisplay.text).ToArray());

            originalRandomWordValue = randomWordDisplay.text;
            randomWordDisplay.text = String.Empty; // Clean up the word display's value
            randomWordDisplay.transform.GetChild(0).gameObject.SetActive(true); // Activate the word display's background
        }

        private void PopulateAliases(string[] aliases)
        {
            // Randomize the order of the aliases
            aliases.Shuffle();
            
            // Instantiate the word aliases
            foreach (string alias in aliases)
            {
                TMP_Text wordAliasDisplay = Instantiate(
                    wordAliasPrefab,
                    mainCamera.WorldToScreenPoint(Vector3.zero),
                    Quaternion.identity,
                    canvas);
                
                wordAliasDisplay.text = alias;
                wordAliases.Add(wordAliasDisplay);
            }
        }

        private void Update()
        {
            // If mouse is pressed, attempt to grab a word alias
            if (Input.GetMouseButtonDown(0))
            {
                Grab();
            }
            
            // Early exit since both statements below require a grabbed word alias
            if (grabbedWordAliasTransform == null) return;
            
            // Move the grabbed word alias
            if (Input.GetMouseButton(0))
            {
                grabbedWordAliasTransform.position = Input.mousePosition + (Vector3) grabOffset;
            }
                
            Debug.DrawLine(grabbedWordAliasTransform.position - wordAliasLabelWidth, randomWordDisplay.transform.position - wordAliasLabelWidth, Color.red);
            
            // If mouse is released, check if the grabbed word alias is close enough to the random word display
            if (Input.GetMouseButtonUp(0))
            {
                float distance = Vector2.Distance(grabbedWordAliasTransform.position - wordAliasLabelWidth, randomWordDisplay.transform.position - wordAliasLabelWidth);
                if (distance < wordSnapDistance)
                {
                    Submit();
                }
                    
                grabbedWordAliasTransform = null;
            }
        }

        private void Grab()
        {
            // Initialize UI raycast
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            // Raycast
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, raycastResults);
            
            // Check if the raycast hit a word alias
            if (raycastResults.Count > 0 && raycastResults.FirstOrDefault().gameObject.TryGetComponent(out WordAlias wordAlias))
            {
                grabOffset = wordAlias.transform.position - Input.mousePosition;
                grabbedWordAliasTransform = wordAlias.transform;
                
                wordAlias.OnWordGrabbed();
            }
        }

        private void Submit()
        {
            // Extract the word alias string value
            string submittedAlias = grabbedWordAliasTransform.GetComponent<TMP_Text>().text;

            // Destroy word aliases
            foreach (TMP_Text wordAlias in wordAliases)
            {
                Destroy(wordAlias.gameObject);
            }
            
            randomWordDisplay.text = submittedAlias; // Update word display
            randomWordDisplay.transform.GetChild(0).gameObject.SetActive(false); // Deactivate the word display's background
            
            wordSubmittedUnityEvent?.Invoke(submittedAlias.Trim() == originalRandomWordValue);
        }
    }
}
