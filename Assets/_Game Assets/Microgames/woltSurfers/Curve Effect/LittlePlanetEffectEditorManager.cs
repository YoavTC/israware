using EditorAttributes;
using UnityEngine;

namespace _Game_Assets.Microgames.woltSurfers
{
    [ExecuteAlways]
    public class LittlePlanetEffectEditorManager : MonoBehaviour
    {
        private const string ENABLE_KEYWORD = "ENABLED";
        
        private void Awake()
        {
            if (Application.isPlaying)
            {
                Shader.EnableKeyword(ENABLE_KEYWORD);
            } else Shader.DisableKeyword(ENABLE_KEYWORD);
        }
    }
}
