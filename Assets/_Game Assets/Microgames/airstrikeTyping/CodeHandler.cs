using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.airstrikeTyping
{
    public class CodeHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private InputHandler inputHandler;

        [SerializeField] private string code;

        private void Start()
        {
            GenerateRandomCode();
            inputHandler.ShowInputField(code);
        }

        [Button]
        private void GenerateRandomCode()
        {
            code = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                char randomChar = Random.Range(0, 2) == 0 
                    ? (char)Random.Range('0', '9' + 1) 
                    : (char)Random.Range('A', 'Z' + 1);
                code += randomChar;
            }
        }
    }
}
