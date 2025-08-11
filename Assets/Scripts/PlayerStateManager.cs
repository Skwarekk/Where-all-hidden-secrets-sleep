using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walking,
        Sprinting,
        CrouchingIdle,
        CrouchingWalking
    }

    public event EventHandler OnStateChanged;
    public static PlayerStateManager Instance { get; private set; }
    private State state;

    private bool shouldSprint;
    private bool shouldCrouch;

    private float moveInputValue;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Instance of PlayerStateManager");
        }
        Instance = this;

        SetState(State.Idle);
        shouldSprint = false;
        shouldCrouch = false;
    }

    private void Start()
    {
        GameInput.Instance.OnSprintPressed += GameInput_OnSprintPressed;
        GameInput.Instance.OnSprintReleased += GameInput_OnSprintReleased;

        GameInput.Instance.OnCrouchPressed += GameInput_OnCrouchPressed;
        GameInput.Instance.OnCrouchReleased += GameInput_OnCrouchReleased;
    }

    private void OnDestroy()
    {
        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnSprintPressed -= GameInput_OnSprintPressed;
            GameInput.Instance.OnSprintReleased -= GameInput_OnSprintReleased;

            GameInput.Instance.OnCrouchPressed -= GameInput_OnCrouchPressed;
            GameInput.Instance.OnCrouchReleased -= GameInput_OnCrouchReleased;
        }
    }

    // ======= SPRINT EVENTS =======

    private void GameInput_OnSprintPressed(object sender, EventArgs e)
    {
        shouldSprint = true;
    }

    private void GameInput_OnSprintReleased(object sender, EventArgs e)
    {
        shouldSprint = false;
    }

    // ======= CROUCH EVENTS =======

    private void GameInput_OnCrouchPressed(object sender, EventArgs e)
    {
        shouldCrouch = true;
    }

    private void GameInput_OnCrouchReleased(object sender, EventArgs e)
    {
        shouldCrouch = false;
    }

    // ======= STATE MANAGEMENT =======

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateState(float moveInputValue, bool canStandUp)
    {
        this.moveInputValue = moveInputValue;

        State newState;

        bool isMoving = moveInputValue != 0;
        bool isSprinting = isMoving && shouldSprint;
        bool isCrouching = shouldCrouch;

        if (!canStandUp)
        {
            isCrouching = true;
        }

        if (isSprinting && !isCrouching)
        {
            newState = State.Sprinting;
        }
        else if (isCrouching)
        {
            newState = isMoving ? State.CrouchingWalking : State.CrouchingIdle;
        }
        else if (isMoving)
        {
            newState = State.Walking;
        }
        else
        {
            newState = State.Idle;
        }

        if (state != newState)
        {
            SetState(newState);
        }
    }

    // ======= STATE CHECKS =======

    public bool IsIdle()
    {
        return state == State.Idle;
    }

    public bool IsWalking()
    {
        return state == State.Walking;
    }

    public bool IsSprinting()
    {
        return state == State.Sprinting;
    }

    public bool IsCrouchingIdle()
    {
        return state == State.CrouchingIdle;
    }

    public bool IsCrouchingWalking()
    {
        return state == State.CrouchingWalking;
    }

    // ======= COMPOSITE STATE CHECKS =======

    public bool IsCrouching()
    {
        return IsCrouchingIdle() || IsCrouchingWalking();
    }

    public bool IsMoving()
    {
        return IsWalking() || IsSprinting() || IsCrouchingWalking();
    }

    // ======= GETTERS =======

    public float GetMoveInputValue()
    {
        return moveInputValue;
    }
}
