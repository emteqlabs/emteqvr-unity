using System;
using System.Collections;
using EmteqLabs.Models;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EmteqLabs
{
    public class CustomCalibration : MonoBehaviour
    {
        // [SerializeField] 
        // private float _initialRelaxationTimeInSeconds = 160;
        [SerializeField] 
        private int _numberCalibrationSteps = 6;
        [SerializeField] 
        private string[] _calibrationStepInstructions;
        [SerializeField] 
        private string[] _calibrationStepNames;
        [SerializeField] 
        private ExpressionAvatar[] _calibrationExpressions;
        [SerializeField] 
        private int _recordingTime = 3;
        [SerializeField] 
        private GameObject _rootCanvas;
        [SerializeField] 
        private Button _recordingButton;
        [SerializeField] 
        private Text _recordingCountdownTextfield;
        [SerializeField] 
        private Text _calibrationStepInstructionsTextfield;
        [SerializeField] 
        private Text _calibrationStepNameTextfield;
        [SerializeField] 
        private GameObject _calibrationStepInstructionsPanel;
        [SerializeField] 
        private Animator _progressUIAnimator;
        [SerializeField] 
        private Slider _calibrationProgressBar;
        [SerializeField] 
        private GameObject _calibrationCompletePanel;
        [SerializeField]
        private int _currentCalibrationStep;
        private EmgCalibrationData _emgCalibrationData;
        private string _currentCalibrationStepInstructions;
        private string _currentCalibrationStepName;
        private int _currentCountdownTime = 0;
        // Subscribe to this to remove the panel once calibration is finished
        public delegate void OnExpressionCalibrationCompleteDelegate (EmgCalibrationData expressionCalibrationData);
        public event OnExpressionCalibrationCompleteDelegate OnExpressionCalibrationComplete;

        void Start()
        {
            Initialise();
            UpdateCalibrationStepText();
            ChangeExpressionAvatar();
        }

        private void Initialise()
        {
            _calibrationExpressions ??= new ExpressionAvatar[_numberCalibrationSteps];
            _calibrationStepInstructions ??= new string[_numberCalibrationSteps];
            _calibrationStepNames ??= new string[_numberCalibrationSteps];
            _rootCanvas.SetActive(true);
            _currentCalibrationStep = 0;
            _calibrationProgressBar.value = 0;
        }
        
        private void UpdateCalibrationStepText()
        {
            _currentCalibrationStepInstructions = _calibrationStepInstructions[_currentCalibrationStep];
            _currentCalibrationStepName = _calibrationStepNames[_currentCalibrationStep];
            _calibrationStepInstructionsTextfield.text = _currentCalibrationStepInstructions;
            _calibrationStepNameTextfield.text = _currentCalibrationStepName;
        }
        
        private void ChangeExpressionAvatar()
        {
            ResetExpressionAvatars();
            EnableNextExpressionAvatar();
        }
        
        private void ResetExpressionAvatars()
        {
            foreach (ExpressionAvatar expressionAvatar in _calibrationExpressions)
            {
                expressionAvatar.gameObject.SetActive(false);
            }
        }
        
        private void EnableNextExpressionAvatar()
        {
            _calibrationExpressions[_currentCalibrationStep].gameObject.SetActive(true);
            _calibrationExpressions[_currentCalibrationStep].GetComponent<ExpressionAvatar>().ChangeExpression();
        }

        public void RecordExpression()
        {
            StartCoroutine("HandleCalibrationProgress");
            EnableCalibrationStepUI(false);
        }

        private IEnumerator HandleCalibrationProgress()
        {
            InitialiseCountdownTime();
            AnimateCalibrationStepUI("Recording");
            
            EmteqVRManager.StartExpressionCalibration(_calibrationExpressions[_currentCalibrationStep].SelectedExpression);
            
            for (int i = 0; i < _recordingTime; i++)
            {
                DecreaseCountdownTimer();
                yield return new WaitForSeconds(1f);

                if (_currentCountdownTime == 0)
                {
                    if (_currentCalibrationStep < _numberCalibrationSteps - 1)
                    {
                        EndCurrentExpression();
                        UpdateCalibrationStepText();
                        ResetCountdownTimer();
                        AnimateCalibrationStepUI("Finished");
                        EnableCalibrationStepUI(true);
                        ChangeExpressionAvatar();
                    }
                    else
                    {
                        EndCalibration();
                        EnableCalibrationCompletePanel();
                    }
                }
            }
        }

        private void InitialiseCountdownTime()
        {
            _currentCountdownTime = _recordingTime;
        }

        private void EnableCalibrationStepUI(bool enabled)
        {
            _calibrationStepInstructionsPanel.SetActive(enabled);
            _recordingButton.gameObject.SetActive(enabled);
        }

        private void AnimateCalibrationStepUI(string animationTrigger)
        {
            _progressUIAnimator.SetTrigger(animationTrigger);
        }
        
        private void DecreaseCountdownTimer()
        {
            _recordingCountdownTextfield.text = _currentCountdownTime.ToString();
            _currentCountdownTime--;
        }
        
        public void EndCurrentExpression()
        {
            EmteqVRManager.EndExpressionCalibration();
            MoveToNextCalibrationStep();
            UpdateCalibrationProgressBar((float) _currentCalibrationStep / (_numberCalibrationSteps));
        }
        
        private void ResetCountdownTimer()
        {
            _recordingCountdownTextfield.text = "";
            _currentCountdownTime = _recordingTime;
        }
        
        public void EndCalibration()
        {
            EmgCalibrationData data = EmteqVRManager.EndExpressionCalibration();
            Logger.LogMessage($"Calibration Complete: {data.ToString()}");

            UpdateCalibrationProgressBar(1);
            AnimateCalibrationStepUI("Complete");
            OnExpressionCalibrationComplete?.Invoke(data);
        }
        
        private void EnableCalibrationCompletePanel()
        {
            _calibrationCompletePanel.SetActive(true);
        }
        
        private void UpdateCalibrationProgressBar(float progress)
        {
            _calibrationProgressBar.value = progress;
        }

        private void MoveToNextCalibrationStep()
        {
            if (_currentCalibrationStep < _numberCalibrationSteps - 1)
            {
                _currentCalibrationStep++;
            }
        }
    }
}