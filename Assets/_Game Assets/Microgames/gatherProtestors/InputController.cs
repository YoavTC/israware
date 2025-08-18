using System;
using System.Collections.Generic;
using System.Linq;
using External_Packages.Extensions;
using External_Packages.Extra_Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.gatherProtestors
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Sprite[] protestorSprites;
        private List<SpriteRenderer> protestors;
        
        [SerializeField] private UnityEvent<float> showProtestorUnityEvent;
        [SerializeField] private UnityEvent showedAllProtestorsUnityEvent;

        [Serializable]
        public class ColorSet
        {
            [SerializeField] public Color colorA;
            [SerializeField] public Color colorB;
        }

        [SerializeField] private List<ColorSet> colorSets;
        
        private void Start()
        {
            protestors = GetComponentsInChildren<SpriteRenderer>(true)
                .OrderBy(p => p.transform.position.y)
                .ToList();
        
            ApplyGradientColors();
            SortRenderingLayers();
        
            // showProtestorUnityEvent?.Invoke(protestors.Count);
        }
        
        private void ApplyGradientColors()
        {
            for (int i = 0; i < protestors.Count; i++)
            {
                ColorSet colorSet = colorSets.Random();
                float t = (float)i / (protestors.Count - 1);
                protestors[i].color = Color.Lerp(colorSet.colorA, colorSet.colorB, t);
            }
        }
        
        private void SortRenderingLayers()
        {
            for (int i = 0; i < protestors.Count; i++)
            {
                protestors[i].sortingOrder = protestors.Count - 1 - i;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowProtestor();
            }
        }

        private void ShowProtestor()
        {
            var protestor = protestors[Random.Range(0, protestors.Count)];
            protestor.sprite = protestorSprites[Random.Range(0, protestorSprites.Length)];
            protestor.flipX = Random.value > 0.5f;
            protestor.gameObject.SetActive(true);
            showProtestorUnityEvent?.Invoke(protestors.Count);
            
            protestors.Remove(protestor);
            
            if (protestors.Count == 0)
            {
                ScaleEffectAll();
                showedAllProtestorsUnityEvent?.Invoke();
            }
        }

        private void ScaleEffectAll()
        {
            foreach (var child in transform.Children())
            {
                child.GetOrAdd<TweenScaleEffect>().DoEffect();
            }
        }
    }
}
