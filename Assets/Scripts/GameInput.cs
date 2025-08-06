using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    // Sprint events
    public event EventHandler OnSprintPressed;
    public event EventHandler OnSprintReleased;

    // Crouch events
    public event EventHandler OnCrouchPressed;
    public event EventHandler OnCrouchReleased;

    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Instance of GameInput!");
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Sprint.started += Sprint_started;
        playerInputActions.Player.Sprint.canceled += Sprint_canceled;

        playerInputActions.Player.Crouch.started += Crouch_started;
        playerInputActions.Player.Crouch.canceled += Crouch_canceled;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Sprint.started -= Sprint_started;
        playerInputActions.Player.Sprint.canceled -= Sprint_canceled;

        playerInputActions.Player.Crouch.started -= Crouch_started;
        playerInputActions.Player.Crouch.canceled -= Crouch_canceled;

        playerInputActions.Dispose();
    }

    private void Sprint_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintReleased?.Invoke(this, EventArgs.Empty);
    }

    private void Crouch_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCrouchPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Crouch_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCrouchReleased?.Invoke(this, EventArgs.Empty);
    }

    public float GetInputAxis()
    {
        return playerInputActions.Player.Move.ReadValue<float>();
    }
}
