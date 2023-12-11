using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour
{
    [SerializeField] private float _percentThreshold = 0.2f;
    [SerializeField] private AnimationCurve _swipeCurve;
    [SerializeField] private float _SwipeDuration = 0.5f;
    [SerializeField] private float _deadZone = 20;
    [SerializeField] private float sensitivity = 0.2f;
    private Vector3 _pageHolderPosition;

    private float _screenWidth;

    private Vector2 touchInputStartPos;

    private void Start()
    {
        _pageHolderPosition = transform.position;
        _screenWidth = Screen.width;

        if (Input.touchSupported) Debug.LogWarning("Current config : Touchscreen");
        else Debug.LogWarning("Current config : Mouse");
    }

    private void Update()
    {
        if (Input.touchSupported) HandleTouchInput();
        else HandleMouseInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Vector2 lTouchPos = Input.touches[0].position;
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                touchInputStartPos = lTouchPos;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                float lDelta = (touchInputStartPos.x - lTouchPos.x) * sensitivity;
                if (lDelta > _deadZone)
                {
                    Vector3 lNewPos = _pageHolderPosition - Vector3.right * Mathf.Clamp(lDelta, -_screenWidth, _screenWidth);
                    transform.position = ClampSwipe(lNewPos);
                }                
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                float lPercentage = (touchInputStartPos.x - lTouchPos.x) / _screenWidth;
                if (Mathf.Abs(lPercentage) >= _percentThreshold)
                {
                    Vector3 lNewPosition = _pageHolderPosition;

                    SwipeToDirection(lPercentage);
                }
                else
                {
                    StartCoroutine(SmoothSwipe(transform.position, ClampSwipe(_pageHolderPosition), _SwipeDuration));
                }
            }
        }
    }

    private void HandleMouseInput()
    {
        Vector2 lTouchPos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            touchInputStartPos = lTouchPos;
        }
        else if (Input.GetMouseButton(0))
        {
            float lDelta = (touchInputStartPos.x - lTouchPos.x) * sensitivity;
            if (lDelta > _deadZone)
            {
                Vector3 lNewPos = _pageHolderPosition - Vector3.right * Mathf.Clamp(lDelta, -_screenWidth, _screenWidth);
                transform.position = ClampSwipe(lNewPos);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float lPercentage = (touchInputStartPos.x - lTouchPos.x) / _screenWidth;
            if (Mathf.Abs(lPercentage) >= _percentThreshold)
            {
                Vector3 lNewPosition = _pageHolderPosition;

                SwipeToDirection(lPercentage);
            }
            else
            {
                StartCoroutine(SmoothSwipe(transform.position, ClampSwipe(_pageHolderPosition), _SwipeDuration));
            }
        }

    }


    //public void OnDrag(PointerEventData eventData)
    //{
    //    float lDelta = (eventData.pressPosition.x - eventData.position.x) * sensitivity;
    //    Vector3 lNewPos = _pageHolderPosition - Vector3.right * Mathf.Clamp(lDelta, -_screenWidth, _screenWidth);
    //    transform.position = ClampSwipe(lNewPos);
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    float lPercentage = (eventData.pressPosition.x - eventData.position.x) / _screenWidth;
    //    if (Mathf.Abs(lPercentage) >= _percentThreshold)
    //    {
    //        Vector3 lNewPosition = _pageHolderPosition;

    //        SwipeToDirection(lPercentage);
    //    }
    //    else
    //    {
    //        StartCoroutine(SmoothSwipe(transform.position, ClampSwipe(_pageHolderPosition), _SwipeDuration));
    //    }
    //}

    private IEnumerator SmoothSwipe(Vector3 startPos, Vector3 endPos, float duration)
    {
        float lEt = 0;
        while (lEt <= duration)
        {
            lEt += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, _swipeCurve.Evaluate(lEt / duration));
            yield return new WaitForEndOfFrame();
        }
    }

    private Vector3 ClampSwipe(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -_screenWidth / 2, _screenWidth / 2);
        return position;
    }

    /// <summary>
    /// Swipe the screen to a given direction. Scrolls right if pDirection >= 0, scrolls left otherwise.
    /// </summary>
    /// <param name="pDirection"></param>
    public void SwipeToDirection(float pDirection)
    {
        Vector3 lNewPosition = _pageHolderPosition;

        if (pDirection >= 0)
            lNewPosition += Vector3.left * _screenWidth;
        else
            lNewPosition += Vector3.right * _screenWidth;

        lNewPosition = ClampSwipe(lNewPosition);

        StartCoroutine(SmoothSwipe(transform.position, lNewPosition, _SwipeDuration));
        _pageHolderPosition = lNewPosition;
    }
}
