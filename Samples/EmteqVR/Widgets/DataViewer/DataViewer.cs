using System.Collections.Generic;
using EmteqLabs.Faceplate;
using EmteqLabs.MaskProtocol;
using UnityEngine;

namespace EmteqLabs
{
    public class DataViewer : MonoBehaviour
    {
        [SerializeField]
        private List<SensorGUI> _sensors;

        private Dictionary<MuscleMapping, ushort> _emgAmplitudeRms;

        private void Start()
        {
            EmteqVRManager.OnSensorContactStateChange += OnSensorContactStateChange;
        }

        void Update()
        {
            _emgAmplitudeRms = EmteqVRManager.GetEmgAmplitudeRms();
            foreach(SensorGUI sensor in _sensors)
            {
                sensor.SetSensorValue(_emgAmplitudeRms[sensor.SensorName]);
            }
        }

        private void OnDestroy()
        {
            EmteqVRManager.OnSensorContactStateChange -= OnSensorContactStateChange;
        }
        
        private void OnSensorContactStateChange(Dictionary<MuscleMapping, ContactState> sensorcontactstate)
        {
            foreach(SensorGUI sensor in _sensors)
            {
                sensor.SetContactState(sensor.SensorName, sensorcontactstate[sensor.SensorName]);
            }
        }

        void SetVisible(bool value)
        {
            this.gameObject.SetActive(value);
        }
    }
}