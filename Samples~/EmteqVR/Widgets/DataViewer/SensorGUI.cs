using EmteqLabs.Faceplate;
using EmteqLabs.MaskProtocol;
using UnityEngine;
using UnityEngine.UI;

namespace EmteqLabs
{
    public class SensorGUI : MonoBehaviour
    {
        public MuscleMapping SensorName;
        [SerializeField]
        private Text _displayValue;
        [SerializeField]
        private Image _sensorUI;

        public void SetSensorValue(ushort value)
        {
            _displayValue.text = value.ToString();
        }
        
        public void SetContactState(MuscleMapping sensorName, ContactState contactState)
        {
            switch (contactState)
            {
                case ContactState.Off:
                case ContactState.Off_On:
                case ContactState.Off_Stable:
                case ContactState.Stable_Off:
                case ContactState.On_Off:
                    _sensorUI.color = Color.gray;
                    break;
                case ContactState.On:
                case ContactState.On_Stable:
                case ContactState.Stable_On:
                    _sensorUI.color = Color.cyan;
                    break;
                case ContactState.Stable:
                case ContactState.Settled:
                    _sensorUI.color = Color.green;
                    break;
                case ContactState.Fault_Stable:
                case ContactState.Stable_Fault:
                case ContactState.Fault:
                    _sensorUI.color = Color.red;
                    break;
                default:
                    _sensorUI.color = Color.white;
                    break;
            }
        }
    }
}