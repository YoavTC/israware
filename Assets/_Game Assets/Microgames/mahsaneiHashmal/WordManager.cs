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
        
        [SerializeField] private SerializedDictionary<TMP_Text, string[]> wordAliasesDictionary;
        
        [SerializeField] private TMP_Text wordAliasPrefab;
        
        private TMP_Text randomWordDisplay;
        private string originalRandomWordValue;
        private List<TMP_Text> wordAliases = new List<TMP_Text>();

        [SerializeField] private float wordSnapDistance;
        private Transform grabbedWordAliasTransform;
        private Vector2 grabOffset;

        private const float WORD_ALIAS_DISPLAY_WIDTH = 17f;
        private Vector3 wordAliasLabelWidth => new Vector3(0f, WORD_ALIAS_DISPLAY_WIDTH, 0f);
        
        [SerializeField] private UnityEvent<bool> wordSubmittedUnityEvent;

        private void Start()
        {
            randomWordDisplay = wordAliasesDictionary.Keys.ToArray()[Random.Range(0, wordAliasesDictionary.Count)];
            PopulateAliases(wordAliasesDictionary[randomWordDisplay].Append(randomWordDisplay.text).ToArray());

            originalRandomWordValue = randomWordDisplay.text;
            randomWordDisplay.text = String.Empty;
            randomWordDisplay.transform.GetChild(0).gameObject.SetActive(true);
        }

        private void PopulateAliases(string[] aliases)
        {
            aliases.Shuffle();
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
            if (Input.GetMouseButtonDown(0))
            {
                Grab();
            }

            if (grabbedWordAliasTransform != null)
            {
                if (Input.GetMouseButton(0))
                {
                    grabbedWordAliasTransform.position = Input.mousePosition + (Vector3) grabOffset;
                }
                
                Debug.DrawLine(grabbedWordAliasTransform.position - wordAliasLabelWidth, randomWordDisplay.transform.position - wordAliasLabelWidth, Color.red);
                
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
        }

        private void Grab()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, raycastResults);
            
            if (raycastResults.Count > 0 && raycastResults.FirstOrDefault().gameObject.TryGetComponent(out WordAlias wordAlias))
            {
                grabOffset = wordAlias.transform.position - Input.mousePosition;
                
                wordAlias.OnWordGrabbed();
                grabbedWordAliasTransform = wordAlias.transform;
            }
        }

        private void Submit()
        {
            string submittedAlias = grabbedWordAliasTransform.GetComponent<TMP_Text>().text;

            foreach (TMP_Text wordAlias in wordAliases)
            {
                Destroy(wordAlias.gameObject);
            }
            
            randomWordDisplay.text = submittedAlias;
            wordSubmittedUnityEvent?.Invoke(submittedAlias.Trim() == originalRandomWordValue);
        }
    }
}
