using UnityEngine;

namespace MadDuck.Scripts.Character.Module
{
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
