using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    [Serializable]
    public class InputButton
    {
        public Coroutine buttonCoroutine;
        public bool isDown;
        public bool isUp;
        public bool isHeld;
    }
    
    [SerializeField, Tooltip("Input reader scriptable object")]
    private InputReader inputReader;

    [SerializeField, ReadOnly] private Vector2 movementInput;
    public Vector2 MovementInput => movementInput;
    [SerializeField, ReadOnly] private InputButton attackButton;
    public InputButton AttackButton => attackButton;
    
    [SerializeField, ReadOnly] private InputButton dodgeButton;
    public InputButton DodgeButton => dodgeButton;

    #region Subscriptions

    private void Subscribe()
    {
        inputReader.MoveEvent += HandleMove;
        inputReader.LightAttackEvent += HandleLightAttack;
        inputReader.HeavyAttackEvent += HandleHeavyAttack;
        inputReader.DodgeEvent += HandleDodge;
        inputReader.BlockEvent += HandleBlock;
    }
    
    private void Unsubscribe()
    {
        inputReader.MoveEvent -= HandleMove;
        inputReader.LightAttackEvent -= HandleLightAttack;
        inputReader.HeavyAttackEvent -= HandleHeavyAttack;
        inputReader.DodgeEvent -= HandleDodge;
        inputReader.BlockEvent -= HandleBlock;
    }
    private void OnEnable()
    {
        Subscribe();
    }
    
    private void OnDisable()
    {
        Unsubscribe();
    }
    // public override void OnNetworkSpawn()
    // {
    //     Subscribe();
    // }
    //
    // public override void OnNetworkDespawn()
    // {
    //     Unsubscribe();
    // }
    #endregion

    #region Event Handlers
    private void HandleMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    private void HandleLightAttack(InputAction.CallbackContext context)
    {
        BindButton(attackButton, context);
    }
    
    private void HandleHeavyAttack(InputAction.CallbackContext context)
    {
        BindButton(attackButton, context);
    }
    
    private void HandleDodge(InputAction.CallbackContext context)
    {
        BindButton(dodgeButton, context);
    }
    
    private void HandleBlock(InputAction.CallbackContext context)
    {
        BindButton(attackButton, context);
    }

    private void BindButton(InputButton button, InputAction.CallbackContext context)
    {
        button.isDown = context.performed;
        button.isUp = context.canceled;
        button.isHeld = context.performed;
        if (button.buttonCoroutine != null)
        {
            StopCoroutine(button.buttonCoroutine);
        }
        button.buttonCoroutine = StartCoroutine(ButtonCoroutine(button));
    }

    private IEnumerator ButtonCoroutine(InputButton button)
    {
        yield return new WaitForEndOfFrame();
        button.isDown = false;
        button.isUp = false;
        button.buttonCoroutine = null;
    }
    #endregion
}
