using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MadDuck.Scripts.Character.Module
{
    [Serializable]
    public struct GuardPattern
    {
        [Group("Area"), Required] public GuardArea guardArea;
        [Group("Timing"), Min(0)] public float delay;
        [Group("Timing"), Min(0)] public float duration;
        [Group("Timing"), Min(0)] public float interval;
        [Group("Timing"), Min(0)] public float resetComboTime;
    }
    
    public class CharacterGuardModule : CharacterModule
    {
        [FormerlySerializedAs("basicAttackPatterns")]
        [Title("Settings")]
        [TableList(Draggable = true,
            HideAddButton = false,
            HideRemoveButton = false,
            AlwaysExpanded = false)]
        [SerializeField] private List<GuardPattern> guardPatterns;
        
        [Title("Debug")]
        [SerializeField, DisplayAsString] private int currentPatternIndex;
        [SerializeField, DisplayAsString] private int previousPatternIndex = -1;
        [SerializeField, DisplayAsString] private bool attackReady;
        [SerializeField, DisplayAsString] private float currentInterval;
        [SerializeField, DisplayAsString] private float currentComboTime;
        
        private GuardPattern? CurrentPattern => guardPatterns[currentPatternIndex];
        private GuardPattern? PreviousPattern
        {
            get
            {
                if (previousPatternIndex == -1) return null;
                return guardPatterns[previousPatternIndex];
            }
        }

        private Coroutine attackCoroutine;
        
        private void SetGuard()
        {
            
        }
        
        protected override void HandleInput()
        {
            if (characterHub.CharacterType is not CharacterType.Player) return;
            base.HandleInput();
            if (PlayerInput.BlockButton.isDown)
            {
                SetGuard();
            }
        }
        
        protected IEnumerator AttackCoroutine()
        {
            if (CurrentPattern == null) yield break;
            currentComboTime = 0;
            characterHub.ChangeActionState(CharacterStates.CharacterActionState.Basic);
            yield return new WaitForSeconds(CurrentPattern.Value.delay);
            CurrentPattern.Value.guardArea.SetActive(true);
            yield return new WaitForSeconds(CurrentPattern.Value.duration);
            CurrentPattern.Value.guardArea.SetActive(false);
            characterHub.ChangeActionState(CharacterStates.CharacterActionState.None);
            previousPatternIndex = currentPatternIndex;
            currentPatternIndex = (currentPatternIndex + 1) % guardPatterns.Count;
            attackReady = false;
            attackCoroutine = null;
        }
    }
    
}
