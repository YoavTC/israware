using Object = System.Object;

namespace _Game_Assets.Scripts.Definitions
{
    public interface IMicrogameCallbacksListener
    {
        public void ReceiveMicrogameCallback(Object parameter)
        {
            if (parameter is MicrogameScriptableObject microgame)
            {
                OnMicrogameLoaded(microgame);
            }
            else if (parameter is bool result)
            {
                OnMicrogameFinished(result);
            }
        }
        
        /// <summary>
        /// Called by the <see cref="GameManager"/> when a microgame is loaded.
        /// </summary>
        /// <param name="microgame"></param>
        public void OnMicrogameLoaded(MicrogameScriptableObject microgame);
        
        /// <summary>
        /// Called by the <see cref="GameManager"/> when a microgame is finished.
        /// </summary>
        /// <param name="result"></param>
        public void OnMicrogameFinished(bool result);
    }
}