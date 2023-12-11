using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenVideo : MonoBehaviour
{
    private RectTransform _rectTransform
    {
        get
        {
            return (RectTransform)transform;
        }
        set
        {
            _rectTransform = value;
        }
    }

    private void Start()
    {
        float lNewScale;
        float lOptimalScaleX;
        float lOptimalScaleY;
        RectTransform lMainCanvasTransform = (RectTransform)MainCanvas.Instance.transform;

        lOptimalScaleX = lMainCanvasTransform.sizeDelta.x / _rectTransform.sizeDelta.y; 
        lOptimalScaleY = lMainCanvasTransform.sizeDelta.y / _rectTransform.sizeDelta.x;

        // Chose the smallest value
        lNewScale = lOptimalScaleX < lOptimalScaleY ? lOptimalScaleX : lOptimalScaleY;

        // Apply scale
        _rectTransform.localScale = Vector3.one * lNewScale;
    }
}
