using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameInput : MonoBehaviour
{
    PlayerControls playerControls;
    public event EventHandler OnInteractAction;
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        playerControls.Player.Interact.performed += Interact_performed;
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
