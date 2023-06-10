using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const String PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance {get; private set;}
    PlayerControls playerControls;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding 
    {
        Move_up,
        Move_down,
        Move_right,
        Move_left,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }
    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerControls.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
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

    public string GetBindingText(Binding binding)
    {
        switch(binding)
        {
            default:
            case Binding.Move_up:
                return playerControls.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_down:
                return playerControls.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_left:
                return playerControls.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_right:
                return playerControls.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerControls.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerControls.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerControls.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerControls.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playerControls.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerControls.Player.Pause.bindings[1].ToDisplayString();
            
        }
    }

    public void rebindBinding(Binding binding, Action onActionRebound)
    {
        playerControls.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch(binding)
        {
            default:
            case Binding.Move_up:
                inputAction = playerControls.Player.Move;
                bindingIndex = 1;
            break;
            case Binding.Move_down:
                inputAction = playerControls.Player.Move;
                bindingIndex = 2;
            break;
            case Binding.Move_left:
                inputAction = playerControls.Player.Move;
                bindingIndex = 3;
            break;
            case Binding.Move_right:
                inputAction = playerControls.Player.Move;
                bindingIndex = 4;
            break;
            case Binding.Interact:
                inputAction = playerControls.Player.Interact;
                bindingIndex = 0;
            break;
            case Binding.InteractAlternate:
                inputAction = playerControls.Player.InteractAlternate;
                bindingIndex = 0;
            break;
            case Binding.Pause:
                inputAction = playerControls.Player.Pause;
                bindingIndex = 0;
            break;
            case Binding.Gamepad_Interact:
                inputAction = playerControls.Player.Interact;
                bindingIndex = 1;
            break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playerControls.Player.InteractAlternate;
                bindingIndex = 1;
            break;
            case Binding.Gamepad_Pause:
                inputAction = playerControls.Player.Pause;
                bindingIndex = 1;
            break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => 
        {
            callback.Dispose();
            playerControls.Player.Enable();
            onActionRebound();
            
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerControls.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
