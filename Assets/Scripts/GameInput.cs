using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}
    PlayerControls playerControls;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        playerControls.Player.Interact.performed += Interact_performed;
        playerControls.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerControls.Player.Pause.performed += Pause_performed;
    }
    private void OnDestroy()
    {
        playerControls.Player.Interact.performed -= Interact_performed;
        playerControls.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerControls.Player.Pause.performed -= Pause_performed;
        

        playerControls.Dispose();
    }
    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
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
