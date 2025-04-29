using System;
using MadDuck.Scripts.Character;
using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;


namespace MadDuck.Scripts.AI.ActionModule
{
public class TrackTarget : CharacterModule
{
    [Title("AI Settings")] 
    [SerializeField] private CharacterMovementModule characterMovement;
    [SerializeField] private CharacterCheckCreatureModule checkCreatureModule;
    
    [SerializeField] public Transform target;

    [SerializeField] private CharacterBasicAttackModule isBasicAttack;
    [SerializeField] private CharacterHeavyAttackModule isHeavyAttack;
    
    [Title("Set Range")] 
    [SerializeField] private float atkRange;
    [SerializeField] private float outLimitRange;

    [Title("Set Timer")] 
    [SerializeField] private float atkRangeTimer;
    [SerializeField] private float attackCooldown = 1.5f;
    private float attackCooldownTimer = 0f;

    [Title("DebugLog")] 
    [SerializeField, DisplayAsString] private float outRangeTimer;
    [SerializeField, DisplayAsString] private float outAtkRangeTimer;
    [SerializeField, DisplayAsString] public bool InRangeAttack;
    [SerializeField, DisplayAsString] public float aiToTargetDistance;
    [SerializeField, DisplayAsString] public bool isAttacking;

    protected override void UpdateModule()
    {
        base.UpdateModule();

        aiToTargetDistance = Vector2.Distance(transform.position, target.position);

        OutOfAttackRange();
        CheckPlayer();

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (InRangeAttack && attackCooldownTimer <= 0)
        {
            isAttacking = true;
            attackCooldownTimer = attackCooldown;
        }
        else { isAttacking = false; }
    }

    private void OutOfAttackRange()
    {
        if (aiToTargetDistance > outLimitRange)
            MoveConditions(false, true);

        if (aiToTargetDistance > atkRange)
        {
            outAtkRangeTimer += Time.deltaTime;
            if (outAtkRangeTimer < atkRangeTimer) return;
            
            MoveConditions(true, false);
        }
        else
        {
            MoveConditions(false, false);
            outAtkRangeTimer = 0;
        }
    }

    private void CheckPlayer()
    {
        InRangeAttack = aiToTargetDistance <= atkRange;
    }

    private void MoveConditions(bool isWalking, bool isRun)
    {
        if (isWalking && !isRun)
        {
            characterMovement.SetDirection(target.position - transform.position);
        }
        else if (!isWalking && isRun)
        {
            characterMovement.isRunning = true;
            characterMovement.SetDirection(target.position - transform.position);
        }
        else if (!isWalking && !isRun)
        {
            characterMovement.SetDirection(Vector2.zero);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, outLimitRange);
    }
}
}