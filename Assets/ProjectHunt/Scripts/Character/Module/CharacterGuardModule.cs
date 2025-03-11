using System;
using TriInspector;
using UnityEngine;

namespace MadDuck.Scripts.Character.Module
{
    [Serializable]
    public struct GuardPattern
    {
        [Group("Area"), Required] public GuardArea GuardArea;
        [Group("Timing"), Min(0)] public float delay;
        [Group("Timing"), Min(0)] public float duration;
        [Group("Timing"), Min(0)] public float interval;
        [Group("Timing"), Min(0)] public float resetComboTime;
    }
    
    public class CharacterGuardModule : CharacterModule
    {
        
        
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
    }
    
}
