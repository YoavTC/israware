using TMPro;
using UnityEngine;

namespace _Game_Assets.Microgames.gatherProtestors
{
    public class RemainingProtestorsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text protestorsTextDisplay;
        [SerializeField] private string protestorsTextFormat;
        private int initialCount;

        private void Start()
        {
            initialCount = -1;
        }

        public void ProtestorShowed(int count)
        {
            if (initialCount == -1)
            {
                initialCount = count;
                count++;
            }
            protestorsTextDisplay.text = string.Format(protestorsTextFormat, count - 1, initialCount);
        }
    }
}
