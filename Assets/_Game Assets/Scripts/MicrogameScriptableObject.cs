using NaughtyAttributes;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    [CreateAssetMenu(menuName = "New Microgame Asset", fileName = "Microgame Asset")]
    public class MicrogameScriptableObject : ScriptableObject
    {
        public string id;
        [Scene] public string sceneName;
    }
}