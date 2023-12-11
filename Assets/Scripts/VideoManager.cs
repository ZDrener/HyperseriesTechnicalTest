using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static UnityEvent ON_TriggerFullscreen = new UnityEvent();
    public static UnityEvent ON_UpdatePlayButton = new UnityEvent();
    public static UnityEvent ON_ResetWaitForFade = new UnityEvent();
    public static UnityEvent<double, double> ON_UpdateTime = new UnityEvent<double, double>();
    public static bool isPlaying = false;

    [SerializeField] private VideoPlayer _videoPlayer;  

    public void PlayPause()
    {
        if (_videoPlayer.isPlaying) 
        {
            _videoPlayer.Pause();
            isPlaying = false;
            ON_UpdatePlayButton.Invoke();
        }
        else
        {
            _videoPlayer.Play();
            StartCoroutine(VideoCoroutine());
            isPlaying = true;
            ON_UpdatePlayButton.Invoke();
        }        
    }

    public void ToggleFullScreen()
    {
        ON_TriggerFullscreen.Invoke();
        ON_UpdatePlayButton.Invoke();
    }

    public void ResetOverlayFade()
    {
        ON_ResetWaitForFade.Invoke();
    }

    private IEnumerator VideoCoroutine()
    {
        // Wait a bit if the video lags and can't start frame 1
        while (!_videoPlayer.isPlaying) yield return new WaitForEndOfFrame();
        
        while (_videoPlayer.isPlaying)
        {
            ON_UpdateTime.Invoke(_videoPlayer.time, _videoPlayer.length);
            yield return new WaitForEndOfFrame();
        }
    }
}
