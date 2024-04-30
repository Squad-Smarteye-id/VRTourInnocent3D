using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace VirtualInnocent.Video
{
    public class VideoSlider : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer videoPlayer;

        [SerializeField]
        private Slider sliderProgress;

        private void Start()
        {
            if (videoPlayer.frameCount > 0)
            {
                sliderProgress.SetValueWithoutNotify((float)videoPlayer.frame / (float)videoPlayer.frameCount);
            }
        }

        private void Update()
        {
            if (videoPlayer.frameCount > 0)
            {
                sliderProgress.SetValueWithoutNotify((float)videoPlayer.frame / (float)videoPlayer.frameCount);
            }
        }

        public void HandleSliderChange(float value)
        {
            SkipToPercent(value);
        }

        private void SkipToPercent(float pct)
        {
            var frame = videoPlayer.frameCount * pct;
            videoPlayer.frame = (long)frame;
        }
    }
}