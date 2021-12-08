using EmteqLabs.Models;
using EmteqLabs.Video;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace EmteqLabs
{
    public class AffectiveVideoDemo : MonoBehaviour
    {
        [FormerlySerializedAs("_calibrationPanel")] [SerializeField] private CustomCalibration customCalibration;
        [SerializeField] public SrtVideoPlayer _videoPlayer;

        private void Awake()
        {
            customCalibration.OnExpressionCalibrationComplete += OnExpressionCalibrationComplete;
        }

        private void OnDestroy()
        {
            _videoPlayer.OnShowSubtitle -= OnShowSubtitle;
        }

        private void OnExpressionCalibrationComplete(EmgCalibrationData expressionCalibrationData)
        {
            customCalibration.OnExpressionCalibrationComplete -= OnExpressionCalibrationComplete;
            customCalibration.gameObject.SetActive(false);
            _videoPlayer.gameObject.SetActive(true);
            PlayVideo();
        }

        private void PlayVideo()
        {
            _videoPlayer.OnShowSubtitle += OnShowSubtitle;
            _videoPlayer.OnHideSubtitle += OnHideSubtitle;
            _videoPlayer.OnVideoClipFinished += OnVideoClipFinished;
            _videoPlayer.Play();
        }
        
        private void OnShowSubtitle(string subtitle)
        {
            //Debug.Log(subtitle+" Show");
            EmteqVRManager.StartDataSection(subtitle);
        }

        private void OnHideSubtitle(string subtitle)
        {
            //Debug.Log(subtitle+" Hide");
            EmteqVRManager.EndDataSection(subtitle);
        }

        private void OnVideoClipFinished(VideoClip videoClip)
        {
            _videoPlayer.OnShowSubtitle -= OnShowSubtitle;
            _videoPlayer.OnHideSubtitle -= OnHideSubtitle;
            _videoPlayer.OnVideoClipFinished -= OnVideoClipFinished;
        }
    }
}
