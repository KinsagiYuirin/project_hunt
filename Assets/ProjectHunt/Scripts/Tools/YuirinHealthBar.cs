using UnityEngine;

public class YuirinHealthBar : MonoBehaviour
{
    [SerializeField] private YuirinBar ProgressTracker;
    
    [SerializeField] private Transform healthBar;
    [SerializeField] private Transform healthFill;
    [SerializeField] private float lerpSpeed = 5f;
    private float fillAmount;

    private void Start()
    {
        fillAmount = healthFill.localScale.x;
    }

    public void SetHealth(float health)
    {
        fillAmount = Mathf.Lerp(fillAmount, health, Time.deltaTime * lerpSpeed);
        healthFill.localScale = new Vector3(fillAmount, healthFill.localScale.y, healthFill.localScale.z);
    }
    
    public virtual void UpdateBar(float currentHealth, float minHealth, float maxHealth, bool isDead)
    {
        
    }
    
}
