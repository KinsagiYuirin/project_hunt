using System.Collections.Generic;
using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;

public class CharacterCheckCreatureModule : CharacterModule
{
    [Title("References")] 
    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private CharacterMovementModule characterMovement;
    [SerializeField] private Animator backStepAnimator;
    
    [Title("Debug")]
    [SerializeField] private Color lineColor = Color.green;
    [SerializeField, DisplayAsString] private List<Vector2> EnemyList;
    [SerializeField, DisplayAsString] private Vector2 closeEnemy;
    [SerializeField, DisplayAsString] private float previousDistance = Mathf.Infinity;
    
    protected override void UpdateModule()
    {
        base.UpdateModule();
        CalculateEnemyDistance();
    }
    
    private void CalculateEnemyDistance()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        var nearestDistance = Mathf.Infinity;
        
        Transform nearestEnemy = null;

        foreach (var hit in hits)
        {
            var dist = Vector2.Distance(transform.position, hit.transform.position);

            if (!(dist < nearestDistance)) continue;
            nearestDistance = dist;
            nearestEnemy = hit.transform;
        }

        if (nearestEnemy != null)
        {
            characterMovement.SpriteRenderer.flipX = !(nearestEnemy.position.x > transform.position.x); // หันขวา
            // หันซ้าย
            
            /*// เปรียบเทียบระยะ
            if (nearestDistance > previousDistance)
            {
                characterMovement.isBackStepping = true;
            }
            else
            {
                characterMovement.isBackStepping  = false;
            }

            previousDistance = nearestDistance;*/
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
