using UnityEngine;
using Unity.WebRTC;

namespace EmteqVR.Runtime.Utilities
{
    [RequireComponent(typeof(AudioListener))]
    public class EmteqStreamListenerTracker : MonoBehaviour
    {
        // This pushes the captured audio from the listener on this object to webrtc
        private void OnAudioFilterRead(float[] data, int channels)
        {
            Audio.Update(data, channels);
        }
        private void OnDestroy()
        {
            VideoStreamManager.Instance.AudioListenerChanged();
        }
    }
}
