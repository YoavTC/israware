using System;
using System.Collections.Generic;
using System.Linq;
using External_Packages.Extra_Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.gatherProtestors
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Sprite[] protestorSprites;
        private List<SpriteRenderer> protestors;
        
        [SerializeField] private UnityEvent<float> showProtestorUnityEvent;
        [SerializeField] private UnityEvent showedAllProtestorsUnityEvent;

        private void Start()
        {
            protestors = GetComponentsInChildren<SpriteRenderer>(true).ToList();
            showProtestorUnityEvent?.Invoke(protestors.Count);
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
                child.GetOrAddComponent<TweenScaleEffect>().DoEffect();
            }
        }
    }
}
