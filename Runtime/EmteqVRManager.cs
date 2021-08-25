using System.Collections.Generic;
using UnityEngine;
using EmteqLabs.Faceplate;
using EmteqLabs.MaskProtocol;
using EmteqLabs.Models;
using UnityEditor;

namespace EmteqLabs
{
    public class EmteqVRManager : MonoBehaviour
    {
        public static EmteqVRManager Instance { get; private set; }
        public static bool ShowContactPrompt = true;
        
        #region  Serialisable properties

        [SerializeField]
        private bool _autoStartRecordingData = true;

        [SerializeField]
        private bool _showContactPrompt = true;

        public bool ShowLogMessages = true;

        #endregion


        #region EmteqVRDevice Public Events

        public static event EmteqVRPlugin.DeviceConnectDelegate OnDeviceConnect
        {
            add
            {
                _onDeviceConnectDelegate += value;
                EmteqVRPlugin.Instance.OnDeviceConnect += OnPluginDeviceConnectHandler;
            }
            remove
            {
                _onDeviceConnectDelegate -= value;
                EmteqVRPlugin.Instance.OnDeviceConnect -= OnPluginDeviceConnectHandler;
            }
        }
        
        private static EmteqVRPlugin.DeviceConnectDelegate _onDeviceConnectDelegate;
        private static void OnPluginDeviceConnectHandler()
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onDeviceConnectDelegate?.Invoke();
            });
        }

        public static event EmteqVRPlugin.DeviceDisconnectDelegate OnDeviceDisconnect
        {
            add
            {
                _onDeviceDisconnectDelegate += value;
                EmteqVRPlugin.Instance.OnDeviceDisconnect += OnPluginDeviceDisconnectHandler;
            }
            remove
            {
                _onDeviceDisconnectDelegate += value;
                EmteqVRPlugin.Instance.OnDeviceDisconnect -= OnPluginDeviceDisconnectHandler;
            }
        }
        private static EmteqVRPlugin.DeviceDisconnectDelegate _onDeviceDisconnectDelegate;
        private static void OnPluginDeviceDisconnectHandler()
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onDeviceDisconnectDelegate?.Invoke();
            });
        }

        public static event EmteqVRPlugin.LogDelegate OnDeviceLog;
        private static void OnLogMessageReceivedPluginHandler(string logMessage, LogType logType)
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                Logger.LogMessage(logMessage, logType);
                OnDeviceLog?.Invoke(logMessage, logType);
            });
        }

        public event EmteqVRPlugin.DeviceFitStateChangeDelegate OnDeviceFitStateChange
        {
            add
            {
                _onDeviceFitStateChange += value;
                EmteqVRPlugin.Instance.OnDeviceFitStateChange += OnPluginDeviceFitStateChangeHandler;
            }
            remove
            {
                _onDeviceFitStateChange -= value;
                EmteqVRPlugin.Instance.OnDeviceFitStateChange -= OnPluginDeviceFitStateChangeHandler;
            }
        }
        
        private static EmteqVRPlugin.DeviceFitStateChangeDelegate _onDeviceFitStateChange;
        private static void OnPluginDeviceFitStateChangeHandler(FitState fitState)
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onDeviceFitStateChange?.Invoke(fitState);
            });
        }

        public static event EmteqVRPlugin.SensorContactStateChangeDelegate OnSensorContactStateChange
        {
            add
            {
                _onSensorContactStateChange += value;
                EmteqVRPlugin.Instance.OnSensorContactStateChange += SensorContactStateChangeHandler;
            }
            remove
            {
                _onSensorContactStateChange -= value;
                EmteqVRPlugin.Instance.OnSensorContactStateChange -= SensorContactStateChangeHandler;
            }
        }
        
        private static event EmteqVRPlugin.SensorContactStateChangeDelegate _onSensorContactStateChange;
        private static void SensorContactStateChangeHandler(Dictionary<MuscleMapping, ContactState> sensorContactState)
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onSensorContactStateChange?.Invoke(sensorContactState);
            });
        }

        public static event EmteqVRPlugin.HeartRateAverageUpdateDelegate OnHeartRateAverageUpdate // once per second
        {
            add
            {
                _onHeartRateAverageUpdate += value;
                EmteqVRPlugin.Instance.OnHeartRateAverageUpdate += OnPluginHeartRateAverageUpdateHandler;
            }
            remove
            {
                _onHeartRateAverageUpdate -= value;
                EmteqVRPlugin.Instance.OnHeartRateAverageUpdate -= OnPluginHeartRateAverageUpdateHandler;
            }
        }
        
        private static event EmteqVRPlugin.HeartRateAverageUpdateDelegate _onHeartRateAverageUpdate;
        private static void OnPluginHeartRateAverageUpdateHandler(double bpm)
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onHeartRateAverageUpdate?.Invoke(bpm);
            });
        }

        public static event EmteqVRPlugin.ValenceUpdateDelegate OnValenceUpdate
        {
            add
            {
                _onValenceUpdate += value;
                EmteqVRPlugin.Instance.OnValenceUpdate += OnPluginValenceUpdateHandler;
            }
            remove
            {
                _onValenceUpdate -= value;
                EmteqVRPlugin.Instance.OnValenceUpdate -= value;
            }
        }
        
        private static event EmteqVRPlugin.ValenceUpdateDelegate _onValenceUpdate;
        private static void OnPluginValenceUpdateHandler(float normalisedValence)
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onValenceUpdate?.Invoke(normalisedValence);
            });
        }

        public static event EmteqVRPlugin.VideoStreamConfigDelegate OnVideoStreamConfig
        {
            add
            {
                _onVideoStreamConfigDelegate += value;
                EmteqVRPlugin.Instance.OnVideoStreamConfig -= OnVideoStreamConfigHandler;
                EmteqVRPlugin.Instance.OnVideoStreamConfig += OnVideoStreamConfigHandler;
            }
            remove
            {
                _onVideoStreamConfigDelegate -= value;
                EmteqVRPlugin.Instance.OnVideoStreamConfig -= OnVideoStreamConfigHandler;
            }
        }
        
        private static EmteqVRPlugin.VideoStreamConfigDelegate _onVideoStreamConfigDelegate;
        private static void OnVideoStreamConfigHandler(string macAddress, string ipAddress, string port)
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onVideoStreamConfigDelegate?.Invoke(macAddress, ipAddress, port);
            });
        }
        
        public static event EmteqVRPlugin.VideoStreamStatusDelegate OnVideoStreamStatus
        {
            add
            {
                _onVideoStreamStatusDelegate += value;
                EmteqVRPlugin.Instance.OnVideoStreamStatus -= OnVideoStreamStatusHandler;
                EmteqVRPlugin.Instance.OnVideoStreamStatus += OnVideoStreamStatusHandler;
            }
            remove
            {
                _onVideoStreamStatusDelegate -= value;
                EmteqVRPlugin.Instance.OnVideoStreamStatus -= OnVideoStreamStatusHandler;
            }
        }
        private static EmteqVRPlugin.VideoStreamStatusDelegate _onVideoStreamStatusDelegate;
        private static void OnVideoStreamStatusHandler(bool isConnected)
        {
            UnityThreadRelay.Instance.Invoke(() =>
            {
                _onVideoStreamStatusDelegate?.Invoke(isConnected);
            });
        }
        #endregion

        #region Unity Life Cycle
        void Awake()
        {
            Application.logMessageReceived += ApplicationOnlogMessageReceived;
            if (FindObjectOfType<EmteqVRManager>() != null && Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            ShowContactPrompt = _showContactPrompt;
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }

        void Start()
        {
            if (Instance == this)
            {
                EmteqVRPlugin.Instance.OnLogMessageReceived += OnLogMessageReceivedPluginHandler;
            }
        }

        void OnEnable()
        {
            if (Instance == this)
            {
                EmteqVRPlugin.Instance.OnDeviceConnect += OnDeviceConnectHandler;
                EmteqVRPlugin.Instance.Enable();
            }
        }

        private void OnDeviceConnectHandler()
        {
            if (_autoStartRecordingData)
            {
                StartRecordingData();
            }
        }

        void OnDisable()
        {
            if (Instance == this)
            {
                StopRecordingData();
                EmteqVRPlugin.Instance.OnDeviceConnect -= OnDeviceConnectHandler;
                EmteqVRPlugin.Instance.Disable();
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Application.logMessageReceived -= ApplicationOnlogMessageReceived;
                EmteqVRPlugin.Instance.OnLogMessageReceived -= OnLogMessageReceivedPluginHandler;
                ShowContactPrompt = true;
            }
        }

        #endregion


        #region Public API Methods

        public static bool IsDeviceConnected()
        {
            return EmteqVRPlugin.Instance.IsDeviceConnected();
        }

        public static Dictionary<MuscleMapping, ushort> GetEmgAmplitudeRms()
        {
            return EmteqVRPlugin.Instance.GetEmgAmplitudeRms();
        }

        public static PpgRawSignal GetRawPpgSignal()
        {
            return EmteqVRPlugin.Instance.GetRawPpgSignal();
        }

        public static void StartRecordingData()
        {
            EmteqVRPlugin.Instance.StartRecordingData();
        }

        public static void StopRecordingData()
        {
            EmteqVRPlugin.Instance.StopRecordingData();
        }

        public static void SetParticipantID(string id)
        {
            EmteqVRPlugin.Instance.SetParticipantID(id);
            ShowContactPrompt = true;
        }

        public static void SetDataPoint(string label)
        {
            EmteqVRPlugin.Instance.SetDataPoint(label);
        }

        public static void SetDataPoint<T>(string label, T metadata)
        {
            EmteqVRPlugin.Instance.SetDataPoint<T>(label, metadata);
        }

        public static void StartDataSection(string label)
        {
            EmteqVRPlugin.Instance.StartDataSection(label);
        }

        public static void StartDataSection<T>(string label, T metadata)
        {
            EmteqVRPlugin.Instance.StartDataSection<T>(label, metadata);
        }

        public static void EndDataSection(string label)
        {
            EmteqVRPlugin.Instance.EndDataSection(label);
        }

        public static void EndDataSection<T>(string label, T metadata)
        {
            EmteqVRPlugin.Instance.EndDataSection<T>(label, metadata);
        }
        
        #endregion


        #region Calibration

        public static void StartExpressionCalibration(ExpressionType expressionType, FaceSide faceSide = FaceSide.Left)
        {
            EmteqVRPlugin.Instance.StartExpressionCalibration(expressionType);
        }

        public static EmgCalibrationData EndExpressionCalibration()
        {
            return EmteqVRPlugin.Instance.EndExpressionCalibration();
        }

        public static void ResetExpressionCalibrationValues()
        {
            EmteqVRPlugin.Instance.ResetExpressionCalibrationValues();
        }

        public static void StartHeartRateBaselineCalibration()
        {
            EmteqVRPlugin.Instance.StartBaselineHeartRateCalibration();
        }

        public static BaselineHeartRateData EndHeartRateBaselineCalibration()
        {
            return EmteqVRPlugin.Instance.EndBaselineHeartRateCalibration();
        }

        public static void ResetBaselineHeartRateCalibration()
        {
            EmteqVRPlugin.Instance.ResetBaselineHeartRateCalibration();
        }

        public static Dictionary<MuscleMapping, float> GetNormalisedEmgRms()
        {
            return EmteqVRPlugin.Instance.GetNormalisedEmgRms();
        }

        public static Dictionary<MuscleMapping, float> GetSustainedNormalisedEmgRms()
        {
            return EmteqVRPlugin.Instance.GetSustainedNormalisedEmgRms();
        }

        #endregion

        private void ApplicationOnlogMessageReceived(string condition, string stacktrace, UnityEngine.LogType type)
        {
            if (type == UnityEngine.LogType.Exception)
            {
                Logger.LogMessage(string.Format("condition: {0} | stacktrace: {1}", condition, stacktrace), LogType.Exception);
            }
        }
    }
}
