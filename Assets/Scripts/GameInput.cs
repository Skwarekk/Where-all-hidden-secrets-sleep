using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnSprintPressed;
    public event EventHandler OnSprintReleased;

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
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Sprint.started -= Sprint_started;
        playerInputActions.Player.Sprint.canceled -= Sprint_canceled;

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

    public float GetInputAxis()
    {
        return playerInputActions.Player.Move.ReadValue<float>();
    }
}
