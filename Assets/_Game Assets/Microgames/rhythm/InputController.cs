using System;
using External_Packages.Extra_Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Assets.Microgames.rhythm
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Transform[] keyTransforms;
        
        [SerializeField] private KeyCode[] keySet;
        [SerializeField] private UnityEvent<int> keyPressedUnityEvent;

        private void Update()
        {
            foreach (KeyCode key in keySet)
            {
                if (Input.GetKeyDown(key))
                {
                    Debug.Log($"Pressed {key}");
                    PressKey(Array.IndexOf(keySet, key));
                }
            }
        }

        private void PressKey(int keyIndex)
        {
            keyPressedUnityEvent.Invoke(keyIndex);
            keyTransforms[keyIndex].GetOrAddComponent<TweenScaleEffect>().DoEffect();
        }
    }
}