using System;
using System.Collections;
using MadDuck.Scripts.BehaviourGraphs.CustomActions;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MadDuck.Scripts.Character.Module.AI
{
    public class CharacterAIModule : CharacterModule
    {
        [Title("AI Settings")]
        [SerializeField] private CharacterMovementModule characterMovement;
        [SerializeField] private Transform target;
        
        [Title("Set Range")]
        [SerializeField] private float atkRange;
        [SerializeField] private float outLimitRange;

        [Title("Set Timer")]
        [SerializeField] private float atkRangeTimer;
        
        [Title("Debug")]
        [SerializeField, DisplayAsString] private float outRangeTimer;
        [SerializeField, DisplayAsString] private float outAtkRangeTimer;
        [SerializeField, DisplayAsString] public bool InRangeAttack;
        [SerializeField, DisplayAsString] private float aiToTargetDistance;
        
        protected override void UpdateModule()
        {
            base.UpdateModule();
            aiToTargetDistance = Vector2.Distance(transform.position, target.position);
                
            OutOfAttackRange();
        }
        
        private void OutOfAttackRange()
        {
                
            if (aiToTargetDistance > outLimitRange)
                MoveConditions(true, false);
                
            if (aiToTargetDistance > atkRange)
            {
                outAtkRangeTimer += Time.deltaTime;
                if (outAtkRangeTimer < atkRangeTimer) return;
                    
                MoveConditions(true, false);
            }
            else
            {
                MoveConditions(false, false);
                InRangeAttack = true;
                outAtkRangeTimer = 0;
            }
        }

        private void MoveConditions(bool isWalking, bool isRun)
        {
            if (isWalking && !isRun)
            {
                characterMovement.SetDirection(target.position - transform.position);
            }
            else
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

