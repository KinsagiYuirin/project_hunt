using MadDuck.Scripts.Utils;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GuardArea : DamageArea
{
    protected override void Start()
    {
        damageCollider = GetComponent<Collider2D>();
        damageCollider.isTrigger = false;
    }
}
