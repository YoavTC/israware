using TMPro;
using UnityEngine;

namespace _Game_Assets.Menu
{
    public class VersionDisplayHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text versionDisplay;
        [SerializeField] private string versionDisplayFormat;
        
        void Start()
        {
            versionDisplay.text = string.Format(versionDisplayFormat, Application.version, Application.unityVersion, Application.platform);
        }
    }
}
