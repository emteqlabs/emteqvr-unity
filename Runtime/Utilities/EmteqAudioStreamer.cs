using System;
using System.Linq;
using Unity.WebRTC;
using UnityEngine;
using Unity.RenderStreaming;

namespace EmteqVR.Runtime.Utilities
{
    public class EmteqAudioStreamer : AudioStreamBase
    {
        private MediaStream m_audioStream;
        private AudioListener m_audioListener;
        
        private EmteqStreamListenerTracker _listenerTracker;

        private bool _lookingForNewListener = false;
        
        void Awake()
        {
            AudioListener currentListener = GameObject.FindObjectOfType<AudioListener>();
            
            if (currentListener != null)
            {
                ConfigureForNewListener(currentListener);
            }
            else
            {
                this._lookingForNewListener = true;
            }
        }

        private void Update()
        {
            if (_lookingForNewListener == true)
            {
                AudioListener currentListener = GameObject.FindObjectOfType<AudioListener>();
                
                if (currentListener != null)
                {
                    this._lookingForNewListener = false;

                    ConfigureForNewListener(currentListener);
                }
            }
        }
        
        void OnDisable()
        {
            Audio.Stop();
        }

        public void AudioListenerChanged()
        {
            this._lookingForNewListener = true;
        }
        
        public void ConfigureForNewListener(AudioListener newAudioListener)
        {
            Destroy(this._listenerTracker);
            
            this._listenerTracker = newAudioListener.gameObject.AddComponent<EmteqStreamListenerTracker>();
            
        }
        
        protected override MediaStreamTrack CreateTrack()
        {
            m_audioStream = Unity.WebRTC.Audio.CaptureStream();
            return m_audioStream.GetTracks().First();
        }

    }
}
