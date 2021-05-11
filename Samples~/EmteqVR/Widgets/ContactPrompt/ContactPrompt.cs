using EmteqLabs.MaskProtocol;
using UnityEngine;

namespace EmteqLabs
{
    public class ContactPrompt : MonoBehaviour
    {
        [SerializeField]
        private FitState _fitStateThreshold = FitState.Average;
        [SerializeField]
        private float _hidePromptIfStableForSeconds = 3;
        [SerializeField]
        private string _promptMessage;
        [SerializeField]
        private EmteqVRMaskGUI _emteqVRMaskGUI;
        
        private float _currentCountdownValue;
        private bool _isEvaluating = false;
        private FitState _currentFitState = FitState.None;

        void Start()
        {
            EmteqVRManager.Instance.OnDeviceFitStateChange += OnFitStateChange;
            _emteqVRMaskGUI.SetInstructions(_promptMessage);
        }
        
        private void Update()
        {
            EvaluateFitStateStability();
        }

        private void OnDestroy()
        {
            EmteqVRManager.Instance.OnDeviceFitStateChange -= OnFitStateChange;
        }

        private void OnFitStateChange(FitState fitState)
        {
            _currentFitState = fitState;
            //if fitstate falls below threshold, prompt VR User
            if (fitState < _fitStateThreshold)
            {
                //trigger only if not already evaluating
                if (!_isEvaluating)
                {
                    StartEvaluatingStability();
                }
            }
            Logger.LogMessage(string.Format("FitState: {0}", _currentFitState.ToString()), LogType.Log);
        }

        private void StartEvaluatingStability()
        {
            if (!_emteqVRMaskGUI.IsActive())
            {
                _emteqVRMaskGUI.Show();
            }
            _currentCountdownValue = _hidePromptIfStableForSeconds;
            _isEvaluating = true;
        }

        private void EvaluateFitStateStability()
        {
            if (_isEvaluating)
            {
                // fitstate must stay stable for a number of seconds
                if (_currentFitState < _fitStateThreshold)
                {
                    StartEvaluatingStability();
                }
                else
                {
                    _currentCountdownValue -= Time.deltaTime;
                    if(_currentCountdownValue <= 0f)
                    {
                        _emteqVRMaskGUI.Hide();
                        _isEvaluating = false;
                    }
                }
            }
        }
    }
}