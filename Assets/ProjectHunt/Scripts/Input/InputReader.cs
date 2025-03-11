using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<InputAction.CallbackContext> MoveEvent;
    public event Action<InputAction.CallbackContext> LightAttackEvent;
    public event Action<InputAction.CallbackContext> HeavyAttackEvent;
    public event Action<InputAction.CallbackContext> DodgeEvent;
    public event Action<InputAction.CallbackContext> BlockEvent;
    
    private Controls _controls;
    
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        
        _controls?.Player.Enable();
    }

    private void OnDisable()
    {
        _controls?.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context);
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        LightAttackEvent?.Invoke(context);
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        HeavyAttackEvent?.Invoke(context);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        DodgeEvent?.Invoke(context);
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        BlockEvent?.Invoke(context);
    }
}

