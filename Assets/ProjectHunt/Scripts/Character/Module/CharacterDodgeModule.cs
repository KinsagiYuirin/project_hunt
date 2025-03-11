using System;
using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace MadDuck.Scripts.Character.Module
{
    public class CharacterDodgeModule : CharacterModule
    {
        [FormerlySerializedAs("rb2d")]
        [Title("Dodge References")]
        [SerializeField] private Rigidbody2D dRb2d;
        [SerializeField] private Animator dodgeAnimator;
        
        [Title("Dodge Settings")]
        [SerializeField, Required] private CharacterHealthModule healthModule;
        [SerializeField, Required] private CharacterMovementModule movementModule;
        [SerializeField] private float dodgeSpeed = 100f;
        [SerializeField] private float dodgeCooldown = 5f;
        [SerializeField] private float dodgeDuration = 0.5f;
        
        [Title("Dodge Debug")]
        [SerializeField, DisplayAsString] private bool dodgeReady;
        private Coroutine dodgeCoroutine;
        [SerializeField, DisplayAsString] private Vector2 dodgeDirection;
        
        private static readonly int IsDodge = Animator.StringToHash("IsMoving");

        private void Start()
        {
            dodgeDirection = Vector2.zero;
            dodgeReady = true;
            dRb2d = movementModule.Rb2d;
        }

        protected override void UpdateModule()
        {
            if (!ModulePermitted) return;
            base.UpdateModule();
        }
        
        public void GetDodge()
        {
            if (!dodgeReady) return;
            if (!ModulePermitted) return;
            if (dodgeCoroutine != null) return;
            
            dodgeCoroutine = StartCoroutine(DodgeCoroutine());
            characterHub.ChangeMovementState(CharacterStates.CharacterMovementState.Dodge);
        }
        
        protected override void HandleInput()
        {
            if (characterHub.CharacterType is not CharacterType.Player) return;
            base.HandleInput();
            if (PlayerInput.DodgeButton.isDown) { GetDodge(); }
        }
        
        /// <summary>
        /// Coroutine that handles the timing of the dodge.
        /// </summary>
        /// <returns></returns>
        protected IEnumerator DodgeCoroutine()
        {
            dodgeReady = false;
            //characterHub.ChangeActionState(CharacterStates.CharacterActionState.Basic);
            
            healthModule.iFrame = true;
            Debug.Log("iFrame is true");

            dodgeDirection = new Vector2(transform.localScale.x, 0).normalized; 
    
            float elapsedTime = 0;
            while (elapsedTime < dodgeDuration)
            {
                transform.position += (Vector3)(dodgeDirection * dodgeSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("iFrame is false");
            healthModule.iFrame = false;
            
            yield return new WaitForSeconds(dodgeCooldown);
            
            //characterHub.ChangeActionState(CharacterStates.CharacterActionState.None);
            dodgeReady = true;
            dodgeCoroutine = null;
        }

        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (dodgeAnimator != null)
                dodgeAnimator.SetBool(IsDodge, PlayerInput.DodgeButton.isDown);
        }
    }
}
