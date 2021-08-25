using UnityEngine;

namespace EmteqVR.Runtime.Utilities
{
    public class EmteqStreamCameraTracker : MonoBehaviour
    {
        private void OnDestroy()
        {
            VideoStreamManager.Instance.MainCameraChanged();
        }
    }
}
