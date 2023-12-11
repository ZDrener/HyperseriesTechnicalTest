using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerPage : MonoBehaviour
{
    private Animator _animator;
    private bool _fullscreen;

    void Start()
    {
        _animator = GetComponent<Animator>();
        VideoManager.ON_TriggerFullscreen.AddListener(TriggerFullscreen);
        ((RectTransform)transform).localPosition = Vector3.right * ((RectTransform)MainCanvas.Instance.transform).sizeDelta.x;
    }

    private void TriggerFullscreen()
    {
        _fullscreen = !_fullscreen;
        _animator.SetBool("Fullscreen", _fullscreen);
    }
}
