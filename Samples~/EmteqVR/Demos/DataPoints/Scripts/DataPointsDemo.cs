using System;
using UnityEngine;

namespace EmteqLabs
{
    public class DataPointsDemo : MonoBehaviour
    {
        [SerializeField] private CubeController _cube;

        public void SetDataPoint()
        {
            var metadata = new CubeData("cyan cube", _cube.transform.position, _cube.transform.rotation, Color.cyan);
            EmteqVRManager.SetDataPoint("cube data", metadata);
            _cube.SetColour(Color.cyan);
        }

        public void StartDataSectionRed()
        {
            var metadata = new CubeData("red cube", _cube.transform.position, _cube.transform.rotation, Color.red);
            EmteqVRManager.StartDataSection("red paused", metadata);
            _cube.PauseRotation();
            _cube.SetColour(Color.red);
        }

        public void EndDataSectionRed()
        {
            var metadata = new CubeData("red cube", _cube.transform.position, _cube.transform.rotation, Color.red);
            EmteqVRManager.EndDataSection("red paused", metadata);
            _cube.ResumeRotation();
            _cube.SetColour(Color.red);
        }

        public void StartDataSectionYellow()
        {
            var metadata = new CubeData("yellow cube", _cube.transform.position, _cube.transform.rotation,
                Color.yellow);
            EmteqVRManager.StartDataSection("yellow paused", metadata);
            _cube.PauseRotation();
            _cube.SetColour(Color.yellow);
        }

        public void EndDataSectionYellow()
        {
            var metadata = new CubeData("yellow cube", _cube.transform.position, _cube.transform.rotation,
                Color.yellow);
            EmteqVRManager.EndDataSection("yellow paused", metadata);
            _cube.ResumeRotation();
            _cube.SetColour(Color.yellow);
        }

        public void StartRecordingData()
        {
            EmteqVRManager.StartRecordingData();
        }

        public void StopRecordingData()
        {
            EmteqVRManager.StopRecordingData();
        }
        

        [Serializable]
        public struct CubeData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Color Colour;
            public string Label;

            public CubeData(string label, Vector3 position, Quaternion rotation, Color color)
            {
                Label = label;
                Position = position;
                Rotation = rotation;
                Colour = color;
            }
        }
    }
}