using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Tproject.AudioManager;
using Seville;
using UnityEngine.Events;

namespace TProject
{
    public class VideoPlayerController : MonoBehaviour
    {
        public VideoClip videoClip;
        public UnityEvent OnVideoFinished;
        [SerializeField] private float hideScreenControlTime = 0f;


        public CanvasRayChecker checker;
        public VideoPlayer videoPlayer;
        public Slider sliderProgress;
        public GameObject controllerGroup;
        public GameObject playButton;
        public GameObject pauseButton;
        public AudioManager audioManager;

        private static List<VideoPlayerController> controllers = new List<VideoPlayerController>();

        private float hideScreenTimer = 0f;

        private void Awake() =>
            controllers.Add(this);


        private void OnDestroy() =>
            controllers.Remove(this);


        private void Start()
        {
            videoPlayer.loopPointReached += CheckEnd;

            if (AudioManager.Instance != null) audioManager = AudioManager.Instance;
            else Debug.LogWarning("please add Audio Manager for the audio video output");

            videoPlayer.clip = videoClip;

            Invoke("GetAudioSource", .5f);
        }

        void GetAudioSource()
        {
            if (AudioManager.Instance != null)
            {
                AudioSource videoAudioSource = AudioManager.Instance.videoSource;

                videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
                videoPlayer.SetTargetAudioSource(0, videoAudioSource);
            }

        }

        private void Update()
        {
            UpdateProgress();
            HandleInput();
            HandleAutoHideController();
        }

        private void UpdateProgress()
        {
            if (videoPlayer.frameCount > 0)
            {
                float progress = (float)videoPlayer.frame / videoPlayer.frameCount;
                sliderProgress.value = progress;
            }
        }

        private void HandleInput()
        {
            if (UserInteract())
            {
                ShowController();
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    SkipTime(450);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    SkipTime(-450);
                if (Input.GetKeyDown(KeyCode.Space))
                    OnClickPlayPause();
            }
        }

        private void HandleAutoHideController()
        {
            if (hideScreenControlTime > 0)
            {
                hideScreenTimer += Time.deltaTime;
                if (hideScreenTimer >= hideScreenControlTime)
                {
                    HideController();
                }
            }
        }

        private void ShowController()
        {
            hideScreenTimer = 0;
            controllerGroup.SetActive(true);
            PlayPauseVisibility();
        }

        private void HideController()
        {
            controllerGroup.SetActive(false);
            hideScreenTimer = 0;
        }

        private void PlayPauseVisibility()
        {
            pauseButton.SetActive(videoPlayer.isPlaying);
            playButton.SetActive(!videoPlayer.isPlaying);
        }

        public void SkipTime(long frameStep)
        {
            videoPlayer.frame += frameStep;
        }

        public void OnClickPlayPause()
        {
            TogglePlayPause();
            PlayPauseVisibility();
        }

        public void PlayVideo()
        {
            foreach (var controller in controllers)
            {
                if (controller != this && controller.videoPlayer.isPlaying)
                {
                    controller.videoPlayer.Pause();
                }
            }

            videoPlayer.Play();
        }

        public void TogglePlayPause()
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                audioManager.musicSource.mute = false;
                // AudioManager.Instance.musicSource.mute = false;
            }
            else
            {
                // videoPlayer.Play();
                PlayVideo();
                audioManager.musicSource.mute = true;
                // AudioManager.Instance.musicSource.mute = true;
            }
        }

        private bool UserInteract()
        {
            return checker.isPlayerHoverCanvas;
        }

        private void CheckEnd(VideoPlayer vp)
        {
            OnVideoFinished?.Invoke();
            // Debug.Log($"video finished");
        }

        public void OnClickForwardTime() =>
            videoPlayer.frame += 450;

        public void OnClickReverseTime() =>
            videoPlayer.frame -= 450;
    }
}