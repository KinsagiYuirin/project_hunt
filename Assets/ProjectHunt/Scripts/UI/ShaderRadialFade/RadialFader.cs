using System;
using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class RadialFader : MonoBehaviour
{
    [Title("Settings")]
    public Material fadeMaterial;
    public float fadeDuration = 1f;
    [SerializeField] private bool needRevert;
    public bool NeedRevert { get => needRevert; set => needRevert = value; }

    [Title("Debug")]
    [SerializeField, DisplayAsString] private bool isFadeFinsh;
    public bool IsFadeFinsh => isFadeFinsh;

    public void StartRadialFadeIn() => StartCoroutine(FadeRadial(2f, 0f));
    public void StartRadialFadeOut() => StartCoroutine(FadeRadial(0f, 2f));

    public void SetUpFadeMaterial()
    {
        switch (needRevert)
        {
            case true:
                fadeMaterial.SetFloat("_Fade", 0f);
                break;
            case false:
                fadeMaterial.SetFloat("_Fade", 2f);
                break;
        }
    }

    IEnumerator FadeRadial(float from, float to)
    {
        isFadeFinsh = false;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(from, to, t / fadeDuration);
            fadeMaterial.SetFloat("_Fade", value);
            yield return null;
        }
        fadeMaterial.SetFloat("_Fade", to);
        isFadeFinsh = true;
    }
}
