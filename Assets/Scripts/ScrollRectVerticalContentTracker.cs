using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectVerticalContentTracker : MonoBehaviour
{

    [SerializeField]
    private float buffer = 500f;

    private ScrollRect scrollRect;

    private float height;
    private Action<float, float> callbacks;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(OnScroll);
        height = scrollRect.GetComponent<RectTransform>().rect.height;
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        OnScroll(Vector2.zero);
    }

    public void RegisterCallback(Action<float, float> callback)
    {
        if (callback == null)
        {
            return;
        }

        callbacks += callback;
    }

    private void OnScroll(Vector2 position)
    {
        var beginning = scrollRect.content.localPosition.y;
        var minRange = beginning - buffer;
        var maxRange = beginning + height + buffer;
        callbacks?.Invoke(minRange, maxRange);
    }

}
