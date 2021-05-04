using EmteqLabs.Models;
using EmteqLabs.Video;
using UnityEngine;
using UnityEngine.Video;

namespace EmteqLabs
{
    public class DataSectionsDemo : MonoBehaviour
    {
        [SerializeField] private CalibrationPanel _calibrationPanel;
        [SerializeField] public SrtVideoPlayer _videoPlayer;

        private void Awake()
        {
            _calibrationPanel.OnExpressionCalibrationComplete += OnExpressionCalibrationComplete;
        }

        private void OnDestroy()
        {
            _videoPlayer.OnShowSubtitle -= OnShowSubtitle;
        }

        private void OnExpressionCalibrationComplete(EmgCalibrationData expressionCalibrationData)
        {
            _calibrationPanel.OnExpressionCalibrationComplete -= OnExpressionCalibrationComplete;
            _calibrationPanel.gameObject.SetActive(false);
            _videoPlayer.gameObject.SetActive(true);
            PlayVideo();
        }

        private void PlayVideo()
        {
            _videoPlayer.OnShowSubtitle += OnShowSubtitle;
            _videoPlayer.OnHideSubtitle += OnHideOnHideSubtitle;
            _videoPlayer.OnVideoClipFinished += OnVideoClipFinished;
            _videoPlayer.Play();
        }
        
        private void OnShowSubtitle(string subtitle)
        {
            //TODO: EmteqManager set event start
        }

        private void OnHideOnHideSubtitle(string subtitle)
        {
            //TODO: EmteqManager set event end
        }

        private void OnVideoClipFinished(VideoClip videoClip)
        {
            _videoPlayer.OnShowSubtitle -= OnShowSubtitle;
            _videoPlayer.OnHideSubtitle -= OnHideOnHideSubtitle;
            _videoPlayer.OnVideoClipFinished -= OnVideoClipFinished;
        }
    }
}
