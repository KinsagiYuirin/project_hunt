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

        [SerializeField, DisplayAsString] private bool IsHeavySwordDrawn;
        [SerializeField, DisplayAsString] private bool IsHeavyAttacking;
        
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
            IsHeavySwordDrawn = false;
            IsHeavyAttacking = false;
            
            if (CurrentPattern == null) yield break;
            currentComboTime = 0;
            characterHub.ChangeActionState(CharacterActionState.Heavy);
            StepAnimation(0);
            yield return new WaitForSeconds(CurrentPattern.Value.delay);
            StepAnimation(1);
            CurrentPattern.Value.damageArea.SetActive(true);
            StepAnimation(2);
            yield return new WaitForSeconds(CurrentPattern.Value.duration);
            StepAnimation(3);
            CurrentPattern.Value.damageArea.SetActive(false);
            characterHub.ChangeActionState(CharacterActionState.None);
            previousPatternIndex = currentPatternIndex;
            currentPatternIndex = (currentPatternIndex + 1) % attackPatterns.Count;
            attackReady = false;
            attackCoroutine = null;
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
