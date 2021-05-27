#if EMTEQVR_EYE
using System.Runtime.InteropServices;
using EmteqLabs.EyeTracking;
using ViveSR.anipal.Eye;
using EyeData_v2 = ViveSR.anipal.Eye.EyeData_v2;
using GazeIndex = ViveSR.anipal.Eye.GazeIndex;
#endif
using UnityEngine;

namespace EmteqLabs
{
    public class EyeTrackingManager : MonoBehaviour
    {
        public delegate void GazeDelegate(TrackedObject trackedObject);
        public static event GazeDelegate OnEnterGaze;
        public static event GazeDelegate OnExitGaze;
        
#if EMTEQVR_EYE
        
        private static EyeData_v2 _eyeData = new EyeData_v2();
        private static bool _eyeCallbackRegistered = false;

        private static byte[] _eyeDataBytes;
        private static TrackedObjectInfo _trackedObjectInfo;
        
        
        private FocusInfo _focusInfo;
        private readonly float _maxDistance = 20;
        private readonly GazeIndex[] _gazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
        
        private Collider _objectInFocus;
        private Collider _previousObjectInFocus;
        private TrackedObject _trackedObject;
        
        private void Start()
        {
            if (!SRanipal_Eye_Framework.Instance.EnableEye)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;

            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && _eyeCallbackRegistered == false)
            {
                SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                _eyeCallbackRegistered = true;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && _eyeCallbackRegistered == true)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                _eyeCallbackRegistered = false;
            }

            bool eyeFocus = false;
            foreach (GazeIndex index in _gazePriority)
            {
                Ray gazeRay;
                int trackedObjectLayer = LayerMask.NameToLayer("Default");
                
                if (_eyeCallbackRegistered)
                    eyeFocus = SRanipal_Eye_v2.Focus(index, out gazeRay, out _focusInfo, 0, _maxDistance, (1 << trackedObjectLayer), _eyeData);
                else
                    eyeFocus = SRanipal_Eye_v2.Focus(index, out gazeRay, out _focusInfo, 0, _maxDistance, (1 << trackedObjectLayer));

                if (eyeFocus)
                {
                    _previousObjectInFocus = _objectInFocus;
                    _objectInFocus = _focusInfo.collider;

                    var previousTrackedObject = _trackedObject;
                    _trackedObject = _focusInfo.transform.GetComponent<TrackedObject>();
                    if (_trackedObject != null)
                    {
                        _trackedObjectInfo = new TrackedObjectInfo(_trackedObject.ObjectId, _focusInfo.distance);
                    }

                    if (_objectInFocus == _previousObjectInFocus)
                    {
                        //do nothing
                    }
                    else
                    {
                        if (previousTrackedObject != null)
                        {
                            ExitGaze(previousTrackedObject);
                        }
                        
                        if (_trackedObject != null)
                        {
                            EnterGaze(_trackedObject);
                        }
                    }
                    break;
                }
            }
            if(!eyeFocus)
            {
                _previousObjectInFocus = null;
                _objectInFocus = null;
                if (_trackedObject != null)
                {
                    ExitGaze(_trackedObject);
                }
                _trackedObject = null;
            }
        }
        
        private void OnDisable()
        {
            Release();
        }

        void OnApplicationQuit()
        {
            Release();
        }
        
        private void EnterGaze(TrackedObject trackedObject)
        {
            trackedObject.EnterGaze(trackedObject.name);
            OnEnterGaze?.Invoke(trackedObject);
        }
        
        private void ExitGaze(TrackedObject trackedObject)
        {
            trackedObject.ExitGaze(trackedObject.name);
            OnExitGaze?.Invoke(trackedObject);
        }

        /// <summary>
        /// Release callback thread when disabled or quit
        /// </summary>
        private static void Release()
        {
            if (_eyeCallbackRegistered == true)
            {
                SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                _eyeCallbackRegistered = false;
            }
        }

        /// <summary>
        /// Required class for IL2CPP scripting backend support
        /// </summary>
        internal class MonoPInvokeCallbackAttribute : System.Attribute
        {
            public MonoPInvokeCallbackAttribute() { }
        }

        /// <summary>
        /// Eye tracking data callback thread.
        /// Callback runs on a separate thread to report at ~120hz.
        /// Note: Unity is not threadsafe and cannot call any UnityEngine api from within callback thread.
        /// MonoPInvokeCallback attribute required for IL2CPP scripting backend
        /// </summary>
        /// <param name="eye_data">Reference to latest eye_data</param>
        [MonoPInvokeCallback]
        private static void EyeCallback(ref EyeData_v2 eye_data)
        {
            _eyeData = eye_data;
            
            byte[] bytesFromStruct = Shared.Common.Helpers.GetBytesFromStruct(eye_data);
            EmteqVRPlugin.Instance.HandleEyeData(bytesFromStruct, _trackedObjectInfo);
            _trackedObjectInfo = new TrackedObjectInfo();
        }
#endif
    }
}