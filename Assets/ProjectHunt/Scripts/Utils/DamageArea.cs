using System;
using MadDuck.Scripts.Utils;
using TriInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class DamageArea : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayer;

    protected Collider2D damageCollider;
    
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
