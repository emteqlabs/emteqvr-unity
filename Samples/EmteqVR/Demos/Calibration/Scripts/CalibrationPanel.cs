using System;
using EmteqLabs.Models;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace EmteqLabs
{
    public class CalibrationPanel : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _rootCanvas;
        [SerializeField] 
        private Button _recordingButton;
        [SerializeField] 
        private PlayableDirector _recordButtonPlayableDirector;
        [SerializeField] 
        private PlayableDirector _calibrationProgressPlayableDirector;

        private int _step;
        private EmgCalibrationData _emgCalibrationData;

        // Subscribe to this to remove the panel once calibration is finished
        
        public delegate void OnExpressionCalibrationCompleteDelegate (EmgCalibrationData expressionCalibrationData);
        public event OnExpressionCalibrationCompleteDelegate OnExpressionCalibrationComplete;

        void Start()
        {
            _rootCanvas.SetActive(true);
            _step = 0;
        }

        //Called from Unity's Timeline
        public void RecordExpression()
        {
            _calibrationProgressPlayableDirector.Play();
            _recordButtonPlayableDirector.Play();
            
            switch (_step)
            {
                case 0:
                    EmteqVRManager.StartExpressionCalibration(ExpressionType.Neutral);
                    break;
                case 1:
                    EmteqVRManager.StartExpressionCalibration(ExpressionType.Smile);
                    break;
                case 2:
                    EmteqVRManager.StartExpressionCalibration(ExpressionType.Neutral);
                    break;
                case 3:
                    EmteqVRManager.StartExpressionCalibration(ExpressionType.Smile);
                    break;
            }
        }

        public void PauseProgressAnimation()
        {
            _calibrationProgressPlayableDirector.Pause();
            EmteqVRManager.EndExpressionCalibration();
            _step++;
        }

        public void EndProgressAnimation()
        {
            EmgCalibrationData data = EmteqVRManager.EndExpressionCalibration();
            Logger.LogMessage($"Calibration Complete: {data.ToString()}");
            OnExpressionCalibrationComplete?.Invoke(data);
        }
    }
}