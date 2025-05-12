using System;
using MadDuck.Scripts.Utils;
using TriInspector;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Collider2D))]
public class DamageArea : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] protected LayerMask targetLayer;
    protected Collider2D damageCollider;
    
    [SerializeField] protected bool shakeCamera;
    [SerializeField, ShowIf("shakeCamera")] protected ScreenShake screenShake;
    [SerializeField, ShowIf("shakeCamera")] protected float shakeDuration = 0.2f;
    [SerializeField, ShowIf("shakeCamera")] protected float shakeMagnitude = 0.4f;
    
    [Title("Audio")]
    [SerializeField] private bool haveAttackSound;
    [SerializeField, ShowIf("haveAttackSound")] private AudioSource attackSound;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private bool activeWhenAttack;
    
    public delegate void OnHit(Collider2D collider);
    public event OnHit OnHitEvent;

    protected virtual void Start()
    {
        damageCollider = GetComponent<Collider2D>();
        damageCollider.isTrigger = true;
    }

    protected virtual void OnDisable()
    {
        OnHitEvent = null;
    }

    public virtual void SetActive(bool active)
    {
        if (!damageCollider) damageCollider = GetComponent<Collider2D>();
        damageCollider.enabled = active;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (shakeCamera)
        {
            screenShake.Shake(shakeDuration, shakeMagnitude);
        }
        
        if (LayerMaskUtils.IsInLayerMask(other.gameObject.layer, targetLayer))
        {
            OnHitEvent?.Invoke(other);
            
            var clip = attackClips[Random.Range(0, attackClips.Length)];
            attackSound.PlayOneShot(clip);
        }
        else
        {
            if (activeWhenAttack)
            {attackSound.PlayOneShot(attackClips[0]);}
            return;
        }
    }
}
