using System.Collections.Generic;
using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;

public class CharacterCheckCreatureModule : CharacterModule
{
    [Title("References")] 
    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask targetLayer;
     
    [Title("Debug")]
    [SerializeField, DisplayAsString] private List<Vector2> EnemyList;
    [SerializeField, DisplayAsString] private Vector2 closeEnemy;
    
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

        foreach (Collider2D hit in hits)
        {
            
            Debug.Log("เจอศัตรู: " + hit.name);
            
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
