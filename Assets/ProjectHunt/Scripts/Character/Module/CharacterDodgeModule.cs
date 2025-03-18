using System;
using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace MadDuck.Scripts.Character.Module
{
    public class CharacterDodgeModule : CharacterModule
    {
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
        [SerializeField, DisplayAsString] private int playerLayer;
        [SerializeField, DisplayAsString] private int enemyLayer;
        [SerializeField, DisplayAsString] private bool dodgeReady;
        private Coroutine dodgeCoroutine;
        [SerializeField, DisplayAsString] private Vector2 dodgeDirection;
        
        private static readonly int IsDash = Animator.StringToHash("IsDash");

        private void Start()
        {
            playerLayer = LayerMask.NameToLayer("Player");
            enemyLayer = LayerMask.NameToLayer("Enemy");
            
            
            
            dodgeDirection = Vector2.zero;
            dodgeReady = true;
        }

        protected override void UpdateModule()
        {
            if (!ModulePermitted) return;
            base.UpdateModule();
            dRb2d = movementModule.Rb2d;
        }
        
        public void GetDodge()
        {
            if (!dodgeReady) return;
            if (!ModulePermitted) return;
            if (dodgeCoroutine != null) return;
            
            dodgeCoroutine = StartCoroutine(DodgeCoroutine());
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
            characterHub.ChangeMovementState(CharacterStates.CharacterMovementState.Dodge);

            healthModule.iFrame = true;
            Debug.Log("iFrame is true");
            
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

            dodgeDirection = movementModule.lastMoveDirection; 
            float elapsedTime = 0;
            while (elapsedTime < dodgeDuration)
            {
                dRb2d.transform.position += (Vector3)(dodgeDirection * dodgeSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            healthModule.iFrame = false;
            Debug.Log("iFrame is false");
            
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
            
            if (movementModule.MoveDirection.magnitude != 0f)
            {
                characterHub.ChangeMovementState(CharacterStates.CharacterMovementState.Walking);
            }
            else
            {
                characterHub.ChangeMovementState(CharacterStates.CharacterMovementState.Idle);
            }
            
            yield return new WaitForSeconds(dodgeCooldown);

            dodgeReady = true;
            dodgeCoroutine = null;
        }

        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (dodgeAnimator == null) {return;}
            if (PlayerInput.DodgeButton.isDown && healthModule.iFrame)
            {
                dodgeAnimator.SetTrigger(IsDash.ToString("IsDash") );
            }
        }
    }
}
