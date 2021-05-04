using System.Collections.Generic;
using EmteqLabs.Faceplate;
using EmteqLabs.MaskProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EmteqLabs
{
    public class EmteqVRMaskGUI : MonoBehaviour
    {
        [SerializeField]
        private VRSensorGUI[] _sensorGUIItems;
        [SerializeField]
        private Text _instructionsText;
        [SerializeField]
        private GameObject _maskMesh;

        private CanvasGroup _maskUI;
        private Dictionary<MuscleMapping, ushort> _emgAmplitudeRms;
        private GameObject _cameraAnchor;

        private static EmteqVRMaskGUI Instance;

        void Awake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            
            //Set Initial State
            _maskMesh.SetActive(false);
            _maskUI = GetComponentInChildren<CanvasGroup>();
            _maskUI.alpha = 0f;
            
            SetMainCameraAsParent();
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            EmteqVRManager.OnSensorContactStateChange += OnSensorContactStateChange;
        }

        private void Update()
        {
            if (_cameraAnchor != null)
            {
                var t = this.transform;
                var to = _cameraAnchor.transform;
                t.position = to.position;
                t.rotation = to.rotation;
                t.localScale = to.localScale;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
            EmteqVRManager.OnSensorContactStateChange -= OnSensorContactStateChange;
        }
        
        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SetMainCameraAsParent();
        }

        private void SetMainCameraAsParent()
        {
            if (Camera.main == null)
            {
                Logger.LogMessage("Could not find Camera tagged as MainCamera - Mask Contact Prompt won't follow head position", LogType.Error);
                return;
            }

            if (_cameraAnchor == null)
            {
                _cameraAnchor = new GameObject();
                _cameraAnchor.name = "EmteqVRMaskGUI Anchor";
            }

            var t = _cameraAnchor.transform;

            if (t.parent == Camera.main.transform) return;
            
            t.parent = Camera.main.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }

        private void OnSensorContactStateChange(Dictionary<MuscleMapping, ContactState> sensorcontactstate)
        {
            for (int i = 0; i < _sensorGUIItems.Length; i++)
            {
                _sensorGUIItems[i].SetContactState(sensorcontactstate[_sensorGUIItems[i].SensorName]);
            }
        }

        public void Show()
        {
            Logger.LogMessage("_maskMesh Show");
            
            //Cancel any active tweens first
            LeanTween.cancel(_maskMesh.gameObject);
            LeanTween.cancel(_maskUI.gameObject);
            
            _maskMesh.SetActive(true);
            LeanTween.moveLocalZ(_maskMesh, 0.35f, 0.3f);
            LeanTween.alphaCanvas(_maskUI, 1f, 0.3f);
        }

        public void Hide()
        {
            Logger.LogMessage("_maskMesh Hide");
            
            //Cancel any active tweens first
            LeanTween.cancel(_maskMesh.gameObject);
            LeanTween.cancel(_maskUI.gameObject);
            
            int meshId = LeanTween.moveLocalZ(_maskMesh, 0f, 0.3f).id;
            LTDescr mDescr = LeanTween.descr(meshId);
            if (mDescr != null)
            {
                // if the tween has already finished it will return null
                // change some parameters
                mDescr.setOnComplete(OnHideTransitionComplete).setEase(LeanTweenType.easeOutBack);
            }

            LeanTween.alphaCanvas(_maskUI, 0f, 0.3f);
        }

        public bool IsActive()
        {
            return _maskMesh.activeInHierarchy;
        }

        private void OnHideTransitionComplete()
        {
            Logger.LogMessage("_maskMesh OnHideTransitionComplete");
            _maskMesh.SetActive(false);
        }

        public void SetInstructions(string instructions)
        {
            instructions = instructions.Replace("\\n", "\n").Replace("<br>", "\n");
            _instructionsText.text = instructions;
        }
    }
}