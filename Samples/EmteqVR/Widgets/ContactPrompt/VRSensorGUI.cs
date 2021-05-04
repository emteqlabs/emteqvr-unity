using System;
using EmteqLabs.Faceplate;
using EmteqLabs.MaskProtocol;
using UnityEngine;

namespace EmteqLabs
{
    public class VRSensorGUI : MonoBehaviour
    {
        public MuscleMapping SensorName;
        
        private MeshRenderer _sensorRenderer;

        private void Awake()
        {
            _sensorRenderer = GetComponent<MeshRenderer>();
        }
        
        public void SetContactState(ContactState contactState)
        {
            switch (contactState)
            {
                case ContactState.Off:
                case ContactState.Off_On:
                case ContactState.Off_Stable:
                case ContactState.Stable_Off:
                case ContactState.On_Off:
                    SetSensorColor(Color.gray);
                    break;
                case ContactState.On:
                case ContactState.On_Stable:
                case ContactState.Stable_On:
                    SetSensorColor(Color.cyan);
                    break;
                case ContactState.Stable:
                case ContactState.Settled:
                    SetSensorColor(Color.green);
                    break;
                case ContactState.Fault_Stable:
                case ContactState.Stable_Fault:
                case ContactState.Fault:
                    SetSensorColor(Color.red);
                    break;
                default:
                    SetSensorColor(Color.white);
                    break;
            }
        }

        private void SetSensorColor(Color colour)
        {
            if (_sensorRenderer == null)
            {
                _sensorRenderer = GetComponent<MeshRenderer>();
            }

            try
            {
                _sensorRenderer.material.color = colour;
            }
            catch (Exception e)
            {
                Logger.LogMessage(string.Format("Object name: {0}", gameObject.name));
                Logger.LogMessage(e.ToString(), LogType.Exception);
                throw;
            }
        }
    }
}