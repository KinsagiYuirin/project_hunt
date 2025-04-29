using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class FadeSlideUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Position Settings")] 
    [SerializeField] private Vector2 startPosition = new Vector2(1000, 0); // เริ่มต้น (ขวา)
    [SerializeField] private Vector2 stayPosition = new Vector2(0, 0);       // ไปกลางจอ
    [SerializeField] private Vector2 endPosition = new Vector2(-1000, 0); //  (ซ้าย)

    [Header("Fade & Slide Settings")]
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private float stayDuration = 2.0f; // เวลาที่อยู่เฉยๆก่อนเริ่ม FadeOut
    public float StayDuration { get => stayDuration; set => stayDuration = value; }

    private float timer = 0f;
    
    private void Awake()
    {
        gameObject.SetActive(true);
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void StartFadeSlide()
    {
        StartCoroutine(FadeSlideRoutine());
    }

    private IEnumerator FadeSlideRoutine()
    {
        // -------------------
        // FadeIn + SlideIn
        yield return StartCoroutine(FadeSlideIn());

        // -------------------
        // Stay (รอค้างกลางจอ)
        yield return new WaitForSeconds(stayDuration);

        // -------------------
        // FadeOut + SlideOut (ย้อนกลับไปทางเดิม)
        yield return StartCoroutine(FadeSlideOut());
    }

    private IEnumerator FadeSlideIn()
    {
        rectTransform.anchoredPosition = startPosition;
        canvasGroup.alpha = 0f;
        
        timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, timer / fadeInDuration);

            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, stayPosition, t);
            canvasGroup.alpha = t;

            yield return null;
        }
        rectTransform.anchoredPosition = stayPosition;
        canvasGroup.alpha = 1f;

    }

    private IEnumerator FadeSlideOut()
    {
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, timer / fadeOutDuration);

            rectTransform.anchoredPosition = Vector2.Lerp(stayPosition, endPosition, t);
            canvasGroup.alpha = 1f - t;

            yield return null;
        }
        rectTransform.anchoredPosition = endPosition;
        canvasGroup.alpha = 0f;
    }
}
