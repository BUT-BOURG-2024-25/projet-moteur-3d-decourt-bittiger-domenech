using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class input_manager : MonoBehaviour
{
    [SerializeField]
    private InputActionReference MovementAction = null;

    [SerializeField]
    private InputActionReference MousePositionAction = null;

    public UnityEngine.Vector3 MovementInput { get; private set; }

    public void RegisterOnClick(Action<InputAction.CallbackContext> OnClickAction, bool register)
    {
        if (register)
        {
            MousePositionAction.action.performed += OnClickAction;
        }
        else
        {
            MousePositionAction.action.performed -= OnClickAction;
        }
    }

    private void Update()
    {
        UnityEngine.Vector2 Movement = MovementAction.action.ReadValue<UnityEngine.Vector2>();
        MovementInput = new UnityEngine.Vector3(Movement.x, 0, Movement.y);
    }


}
