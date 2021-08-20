using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] Image fadeToBlackImage;
    [SerializeField] float transitionSpeed = 1;
    [SerializeField] float startDelay = 3;

    // Start is called before the first frame update
    void Start()
    {
        fadeToBlackImage.color = new Color(0, 0, 0, 1);
        StartCoroutine(Delay(startDelay));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        FadeIn();
    }

    IEnumerator FadeOutCoroutine()
    {
        for (float i = 0; i <= transitionSpeed; i += Time.fixedDeltaTime)
        {
            fadeToBlackImage.color = new Color(0, 0, 0, i / transitionSpeed);
            yield return new WaitForFixedUpdate();
        }
        fadeToBlackImage.color = new Color(0, 0, 0, 1);
    }

    IEnumerator FadeInCoroutine()
    {
        for (float i = transitionSpeed; i >= 0; i -= Time.fixedDeltaTime)
        {
            fadeToBlackImage.color = new Color(0, 0, 0, i / transitionSpeed);
            yield return new WaitForFixedUpdate();
        }
        fadeToBlackImage.color = new Color(0, 0, 0, 0);
    }
}
