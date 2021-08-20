using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroup : MonoBehaviour
{
    [SerializeField] RectTransform activePosition;
    [SerializeField] RectTransform inactivePosition;
    [SerializeField] RectTransform uiComponent;
    [SerializeField] float duration;

    public void SlideIn()
    {
        StartCoroutine(SlideTo(uiComponent, activePosition.anchoredPosition, duration));
    }
    public void SlideOut()
    {
        StartCoroutine(SlideTo(uiComponent, inactivePosition.anchoredPosition, duration));
    }

    IEnumerator SlideTo(RectTransform t, Vector3 endPosition, float duration)
    {
     
        Vector3 startPosition = t.anchoredPosition;

        for (float i = 0; i <= duration; i += 0.01f)
        {
            t.anchoredPosition = Vector3.Lerp(startPosition, endPosition, Mathf.Pow(i / duration, 3));
            yield return new WaitForSecondsRealtime(0.01f);
        }
        t.anchoredPosition = endPosition;
    }
}
