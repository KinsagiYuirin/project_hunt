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
        private static readonly int IsHeavyAttack = Animator.StringToHash("IsHeavyAttack 0");

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
            attackAnimator.SetTrigger(IsHeavyAttack);
            characterHub.ChangeActionState(CharacterActionState.Heavy);
            yield return new WaitForSeconds(CurrentPattern.Value.delay);
            CurrentPattern.Value.damageArea.SetActive(true);
            yield return new WaitForSeconds(CurrentPattern.Value.duration);
            CurrentPattern.Value.damageArea.SetActive(false);
            characterHub.ChangeActionState(CharacterActionState.None);
            previousPatternIndex = currentPatternIndex;
            currentPatternIndex = (currentPatternIndex + 1) % attackPatterns.Count;
            attackReady = false;
            attackCoroutine = null;
            movementModule.isRunning = false;
        }
    }
}
