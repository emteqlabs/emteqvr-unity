using UnityEngine;
using UnityEngine.UI;

namespace EmteqLabs.Video
{
    public class VideoPlayerSubtitle : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
        }

        public void SetText(string text)
        {
            if (_text != null)
            {
                _text.text = text;
            }
        }
    }
}