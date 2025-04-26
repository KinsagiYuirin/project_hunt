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
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 0.8f;
    
    void Update()
    {
        
    }
    
    private void HpLow()
    {
        float alpha = Mathf.PingPong(Time.time * speed, maxAlpha - minAlpha) + minAlpha;
        
        hitEffectColor.a = alpha;
        hitEffectMaterial.SetColor("_EdgeColor", hitEffectColor);
    }
}
