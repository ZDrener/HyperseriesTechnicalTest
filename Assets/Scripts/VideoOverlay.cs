using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoOverlay : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _textVideoDuration;
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Sprite _pauseImage;
    [SerializeField] private Sprite _playImage;
    [Space]
    [SerializeField] private float _timeBeforeOverlayFade = 5;
    [SerializeField] private float _overlayFadeDuration = 0.5f;

    [HideInInspector] public float timeSinceLastInput;
    private CanvasGroup _canvasGroup;


    private void OnEnable()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        VideoManager.ON_UpdateTime.AddListener(UpdateTime);
        VideoManager.ON_ResetWaitForFade.AddListener(ResetWaitForFade);
        VideoManager.ON_UpdatePlayButton.AddListener(UpdatePlayButton);
        UpdatePlayButton();
    }

    public void UpdatePlayButton()
    {
        _buttonPlay.image.sprite = (VideoManager.isPlaying) ? _pauseImage : _playImage;
        if (VideoManager.isPlaying) StartWaitForFade();
        else ResetWaitForFade();
    }

    public void UpdateTime(double currentTime, double videoLength)
    {
        TimeSpan lTimeSpan;
        _progressBar.fillAmount = (float)(currentTime / videoLength);
        lTimeSpan = TimeSpan.FromSeconds(videoLength - currentTime);
        if (lTimeSpan.Hours > 0) _textVideoDuration.text = lTimeSpan.ToString("hh':'mm");
        else _textVideoDuration.text = lTimeSpan.ToString("mm':'ss");
    }

    public void StartWaitForFade()
    {
        timeSinceLastInput = 0;
        _canvasGroup.alpha = 1;
        StartCoroutine(OverlayWaitForFadeCoroutine());
    }
    public void ResetWaitForFade()
    {
        timeSinceLastInput = 0;
        _canvasGroup.alpha = 1;
    }

    public IEnumerator OverlayWaitForFadeCoroutine()
    {
        while (timeSinceLastInput < _timeBeforeOverlayFade)
        {
            timeSinceLastInput += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(OverlayFadeCoroutine());
    }

    private IEnumerator OverlayFadeCoroutine()
    {
        float lEt = 0;
        while (lEt < _overlayFadeDuration)
        {
            lEt += Time.deltaTime;
            _canvasGroup.alpha = 1 - lEt / _overlayFadeDuration;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        VideoManager.ON_UpdateTime.RemoveListener(UpdateTime);
        VideoManager.ON_ResetWaitForFade.RemoveListener(ResetWaitForFade);
        VideoManager.ON_UpdatePlayButton.RemoveListener(UpdatePlayButton);
    }
}
