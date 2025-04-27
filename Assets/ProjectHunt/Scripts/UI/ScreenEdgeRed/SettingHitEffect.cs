using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;

public class SettingHitEffect : MonoBehaviour
{
    [Title("Setting")]
    [SerializeField] private Material hitEffectMaterial;
    [SerializeField] private Color hitEffectColor = Color.red;
    [SerializeField] private CharacterHealthModule characterHealthModule;

    [Header("Hit Effect")]
    [SerializeField] private float speed = 1f;
    [SerializeField, DisplayAsString] private float minAlpha = 0.2f;
    [SerializeField, DisplayAsString] private float healthDifference;
    [SerializeField, DisplayAsString] private float startAlpha = 0;
    
    void Update()
    { 
       HpLow(healthDifference  = (characterHealthModule.pHealthData.maxHealth - characterHealthModule.pHealthData.currentHealth) / 100f);
    }
    
    private void HpLow(float healthDifference)
    {
        minAlpha = Mathf.Round(Mathf.Sqrt(healthDifference / 2f) * 100f) / 100f;
 
        float alpha = Mathf.PingPong(Time.time * speed, healthDifference - startAlpha) + startAlpha;
        
        hitEffectColor.a = alpha;
        hitEffectMaterial.SetColor("_EdgeColor", hitEffectColor);
    }
}
