using System.Collections;
using TriInspector;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    [Title("Fade UI")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private GameObject objectCanvasGroup;
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private float fadeInTime = 1f;
    
    private void Start()
    {
        fadeCanvasGroup.alpha = 1f;
        objectCanvasGroup = gameObject;
    }

    public IEnumerator FadeOut()
    {
        if (objectCanvasGroup.activeSelf == false)
            yield break;
        
        float timer = 0f;
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }
    
    public IEnumerator FadeIn()
    {
        if (objectCanvasGroup.activeSelf == false)
            yield break;
        
        float timer = 0f;
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }
}
