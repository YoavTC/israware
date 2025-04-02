using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Assets.Microgames.airstrikeTyping
{
    public class CodeHandler : MonoBehaviour
    {
        [SerializeField] private string _code;
        
        [Header("Components")]
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private TMP_Text codeCharacterPrefab;

        [Header("Code Showing Settings")] 
        [SerializeField] private Vector2 codeCharacterSpawnOrigin;
        [SerializeField] private Vector2 codeCharacterSpawnRange;
        [SerializeField] private float codeCharacterSpawnDelay;
        [SerializeField] private float codeCharacterSpawnCooldown;
        [SerializeField] private float postCodeShowDelay;
        
        private IEnumerator Start()
        {
            string code = GetRandomCode();
            yield return ShowCode(code);
            inputHandler.ShowInputField(code);
        }
        
        [Button]
        private string GetRandomCode()
        {
            string code = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                char randomChar = Random.Range(0, 2) == 0 
                    ? (char)Random.Range('0', '9' + 1) 
                    : (char)Random.Range('A', 'Z' + 1);
                code += randomChar;
            }

            _code = code;
            return code;
        }

        private IEnumerator ShowCode(string code)
        {
            yield return new WaitForSeconds(codeCharacterSpawnDelay);
            
            WaitForSeconds cooldown = new WaitForSeconds(codeCharacterSpawnCooldown);
            for (int i = 0; i < code.Length; i++)
            {
                Vector2 spawnPosition = new Vector2(codeCharacterSpawnOrigin.x, Random.Range(codeCharacterSpawnRange.x, codeCharacterSpawnRange.y));
                var codeCharacter = Instantiate(codeCharacterPrefab, spawnPosition, Quaternion.identity, transform);
                codeCharacter.text = code[i].ToString();
            
                yield return cooldown;
            }
            
            yield return new WaitForSeconds(postCodeShowDelay);
        }
    }
}
