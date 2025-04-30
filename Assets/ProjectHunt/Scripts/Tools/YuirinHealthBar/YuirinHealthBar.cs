using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

public class YuirinHealthBar : MonoBehaviour
{
    [Title("Health Bar")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float updateSpeed = 0.5f;
    [SerializeField] private float currentHealthPercent = 1f;

    [Header("Details")] 
    [SerializeField] private float firstUpdateSpeed;
    [SerializeField] private bool needSmoothFill = true;
    [SerializeField] private bool needFadeHpBar = true;
    [SerializeField, ShowIf("needFadeHpBar")] private FadeUI hpBarFadeUI;
    
    [Title("Debug")]
    
    
    private Coroutine healthCoroutine;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// เรียกเมื่อ HP เปลี่ยน เพื่ออัปเดตหลอดเลือดแบบ Smooth
    /// </summary>
    public void UpdateHealthUI(float currentHp, float maxHp)
    {
        var targetPercent = Mathf.Clamp01(currentHp / maxHp);
        
        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);
    
        healthCoroutine = StartCoroutine(SmoothFill(targetPercent));
    }

    /// <summary>
    /// ค่อยๆ เปลี่ยนค่าหลอดเลือดแบบลื่น
    /// </summary>
    private IEnumerator SmoothFill(float targetPercent)
    {
        var initialPercent = currentHealthPercent;
        var timer = 0f;

        while (timer < updateSpeed)
        {
            timer += Time.deltaTime;
            currentHealthPercent = Mathf.Lerp(initialPercent, targetPercent, timer / updateSpeed);
            fillImage.fillAmount = currentHealthPercent;
            yield return null;
        }

        currentHealthPercent = targetPercent;
        fillImage.fillAmount = targetPercent;
        healthCoroutine = null;
    }
    
    public IEnumerator FillSmoothly(float target)
    {
        var timer = 0f;
        var start = 0f;
        
        if (needFadeHpBar)
            yield return StartCoroutine(hpBarFadeUI.FadeIn());

        while (timer < firstUpdateSpeed)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, timer / firstUpdateSpeed);
            currentHealthPercent = Mathf.Lerp(start, target, t);
            fillImage.fillAmount = currentHealthPercent;
            yield return null;
        }

        fillImage.fillAmount = target;
    }

    public IEnumerator DrainSmoothly()
    {
        if (needFadeHpBar)
            yield return StartCoroutine(hpBarFadeUI.FadeOut());
    }
}
