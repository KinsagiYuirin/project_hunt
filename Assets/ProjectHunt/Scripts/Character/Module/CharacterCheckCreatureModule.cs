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
    [SerializeField, DisplayAsString] private List<Vector2> EnemyList;
    [SerializeField, DisplayAsString] private Vector2 closeEnemy;
    [SerializeField, DisplayAsString] private float previousDistance = Mathf.Infinity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void UpdateModule()
    {
        base.UpdateModule();
        
        CalculateEnemyDistance();
    }
    
    private void CalculateEnemyDistance()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        float nearestDistance = Mathf.Infinity;
        
        Transform nearestEnemy = null;

        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);

            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearestEnemy = hit.transform;
            }
        }

        if (nearestEnemy != null)
        {
            // หัน sprite ตามตำแหน่งศัตรู
            if (nearestEnemy.position.x > transform.position.x)
            {
                characterMovement.SpriteRenderer.flipX = false; // หันขวา
            }
            else
            {
                characterMovement.SpriteRenderer.flipX = true; // หันซ้าย
            }

            /*// เปรียบเทียบระยะ
            if (nearestDistance > previousDistance)
            {
                characterMovement.isBackStepping = true;
            }
            else
            {
                characterMovement.isBackStepping  = false;
            }

            previousDistance = nearestDistance;

            Debug.Log("ห่างขึ้น? " + characterMovement.isBackStepping );*/
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
