using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine;

    public void FadeToBlack()
    {
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1f);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToTransparent()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0f);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while(!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            Color oldColor = fadeScreen.color;
            // Take a (fadeSpeed * Time.deltaTime) sized step from the current alpha to the target alpha.
            //  This implementation will let us fade out and fade in with the same coroutine.
            float alpha = Mathf.MoveTowards(oldColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            yield return null;
        }
    }
}
