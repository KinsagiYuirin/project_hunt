using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriInspector;
using UnityEngine.Serialization;

namespace MadDuck.Scripts.Character.Module
{
    public class CharacterHeavyAttackModule : CharacterBasicAttackModule
    {
        [Title("Heavy Attack Settings")]
        private static readonly int IsDrawnHeavyAttack = Animator.StringToHash("IsDrawnHeavyAttack");
        private static readonly int IsHeavyAttack = Animator.StringToHash("IsHeavyAttack");

        private bool IsHeavySwordDrawn;
        private bool IsHeavyAttacking;
        
        [SerializeField] private CharacterMovementModule movementModule;
        
        protected override void HandleInput()
        {
            if (characterHub.CharacterType is not CharacterType.Player) return;
            
            if (PlayerInput.HeavyAttackButton.isDown)
            {
                Attack();
            }
        }
        
        protected override IEnumerator AttackCoroutine()
        {
            yield return base.AttackCoroutine();
            movementModule.isRunning = false;
        }

        protected override void StepAnimation(int step)
        {
            if (attackAnimator == null) {return;}
            switch (step)
            {
                case 0:
                    IsHeavySwordDrawn = true;
                    break;
                case 1:
                    IsHeavySwordDrawn = false;
                    break;
                case 2:
                    IsHeavyAttacking = true;
                    break;
                case 3:
                    IsHeavyAttacking = false;
                    break;
            }
        }

        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (attackAnimator == null) {return;}
            attackAnimator.SetBool(IsDrawnHeavyAttack, IsHeavySwordDrawn);    
            attackAnimator.SetBool(IsHeavyAttack, IsHeavyAttacking);
        }
    }
}
