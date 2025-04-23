using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using MadDuck.Scripts.Character.Module;
using UnityEngine.Serialization;

namespace MadDuck.Scripts.Character.Module
{
    /// <summary>
    /// Module responsible for handling character movement.
    /// </summary>
    public class CharacterMovementModule : CharacterModule
    {
        [Title("References")]
        [SerializeField] private Rigidbody2D rb2d;
        public Rigidbody2D Rb2d => rb2d;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        [SerializeField] private Animator walkAnimator;
    
        [Title("Movement Settings")]
        [field: SerializeField] public float MovementSpeed { get; private set; } = 4f;
        [field: SerializeField] public float RunningSpeed { get; private set; } = 2f;
        
        [Title("Movement Debug")]
        [SerializeField, ReadOnly] protected Vector2 moveDirection;
        public Vector2 MoveDirection => moveDirection;
        
        public bool isRunning = false;
        public bool isBackStepping = false;
        public Vector2 lastMoveDirection { get; private set; } = Vector2.right;
        
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsBackStep = Animator.StringToHash("IsBackStep");
        
        /// <summary>
        /// Flips the sprite based on the direction of movement.
        /// </summary>
        public virtual void Flip()
        {
            bool shouldFlip = false;
            
            if (moveDirection.x != 0)
            {
                lastMoveDirection = moveDirection.normalized;

                if (!isBackStepping)
                    shouldFlip = moveDirection.x < 0;
                else
                    shouldFlip = moveDirection.x > 0;

                spriteRenderer.flipX = shouldFlip;
            }
        }

        protected override void UpdateModule()
        {
            base.UpdateModule();

            switch (isRunning)
            {
                case true:
                    rb2d.linearVelocity = moveDirection * MovementSpeed * RunningSpeed;
                    break;
                case false: 
                    rb2d.linearVelocity = moveDirection * MovementSpeed;
                    break;
            }
            Flip();
        }
        
        protected void LateUpdate()
        {
            LateUpdateModule();
        }

        protected override void LateUpdateModule()
        {
            if (moveDirection.magnitude <= 0)
            {
                rb2d.linearVelocity = Vector2.zero;
            }
            base.LateUpdateModule();
        }
        
        /// <summary>
        /// Sets the direction of movement.
        /// </summary>
        /// <param name="direction">Direction of movement.</param>
        public void SetDirection(Vector2 direction, bool forceSet = false)
        {
            if (!ModulePermitted && !forceSet) return;
            moveDirection = direction;
            moveDirection.Normalize();
            if (characterHub.MovementState == CharacterMovementState.Dodge) return;
            var state = moveDirection.magnitude > 0
                ? CharacterMovementState.Walking
                : CharacterMovementState.Idle;
            characterHub.ChangeMovementState(state);
        }
        
        public void SetPosition(Vector2 position)
        {
            if (!ModulePermitted) return;
            rb2d.position = position;
        }
        
        protected override void HandleInput()
        {
            if (characterHub.CharacterType is not CharacterType.Player) return;
            base.HandleInput();
            SetDirection(PlayerInput.MovementInput);
        }
        
        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();

            if (!isBackStepping)
            {
                if (walkAnimator != null)
                    walkAnimator.SetBool(IsMoving, moveDirection.magnitude != 0);
            }
            else
            {
                if (walkAnimator != null)
                    walkAnimator.SetBool(IsBackStep, moveDirection.magnitude != 0);
            }
        }
    }
}

