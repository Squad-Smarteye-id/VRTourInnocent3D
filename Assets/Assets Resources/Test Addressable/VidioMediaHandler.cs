using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Seville;

public class VidioMediaHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private VideoPlayer[] videoPlayers;

    public List<GameObject> uiToShow;

    public CanvasRayChecker checker;

    private bool isPaused = false;

    void Start()
    {
        videoPlayers = FindObjectsOfType<VideoPlayer>();

        isPaused = true;

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer belum di-assign!");
            return;
        }
    }

    private void Update()
    {
        bool isHovered = userInteract();

        SetUIVisibility(isHovered);

    }

    void SetUIVisibility(bool isVisible)
    {
        foreach (GameObject uiObject in uiToShow)
        {
            uiObject.SetActive(isVisible);
        }
    }


    void SkipForward(float seconds)
    {
        if (videoPlayer.canSetTime)
        {
            videoPlayer.time += seconds;
        }
    }

    void SkipPrevious(float seconds)
    {
        if (videoPlayer.canSetTime)
        {
            videoPlayer.time -= seconds;
        }
    }

    public void SkipForwardsec() { SkipForward(5); }
    public void SkipBackward() { SkipPrevious(5); }
    public void pauseVidio()
    {
        if (isPaused)
        {
            PauseAllVidio();
            videoPlayer.Play();
            isPaused = false;
        }
        else
        {
            videoPlayer.Pause();
            isPaused = true;
        }
    }

    public void PauseAllVidio()
    {
        foreach (VideoPlayer player in videoPlayers)
        {
            player.Pause();
        }
    }

    public bool userInteract()
    {
        return checker.isPlayerHoverCanvas;
    }


}
