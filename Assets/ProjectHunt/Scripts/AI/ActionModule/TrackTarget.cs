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

        [SerializeField] public Transform target;

        [SerializeField] private CharacterBasicAttackModule isBasicAttack;
        [SerializeField] private CharacterHeavyAttackModule isHeavyAttack;
        
        [Title("Set Range")] 
        [SerializeField] private float atkRange;
        [SerializeField] private float outLimitRange;

        [Title("Set Timer")] 
        [SerializeField] private float atkRangeTimer;

        [Title("Debug")] 
        [SerializeField, DisplayAsString] private float outRangeTimer;
        
        
        [SerializeField, DisplayAsString] private float outAtkRangeTimer;
        [SerializeField, DisplayAsString] public bool InRangeAttack;
        [SerializeField, DisplayAsString] public float aiToTargetDistance;

        private void Start()
        {
            gameObject.GetComponent<CircleCollider2D>().radius = atkRange;
        }

        protected override void UpdateModule()
        {
            base.UpdateModule();
            
            aiToTargetDistance = Vector2.Distance(transform.position, target.position);

            OutOfAttackRange();
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
                InRangeAttack = true;
                outAtkRangeTimer = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                InRangeAttack = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                InRangeAttack = false;
            }
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