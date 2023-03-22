using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    PlayerControls playerControls;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        playerControls.Player.Interact.performed += Interact_performed;
        playerControls.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }
}
