using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walking,
        Sprinting,
        Crouching
    }

    public event EventHandler OnStateChanged;
    public static PlayerStateManager Instance { get; private set; }
    private State state;

    private bool shouldSprint;
    private bool shouldCrouch;

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

    public void UpdateState(float moveInputValue)
    {
        switch (state)
        {
            case State.Idle:
                if (moveInputValue != 0)
                {
                    SetState(State.Walking);
                }

                if (shouldCrouch)
                {
                    SetState(State.Crouching);
                }

                break;

            case State.Walking:
                if (moveInputValue == 0)
                {
                    SetState(State.Idle);
                }

                if (shouldSprint)
                {
                    SetState(State.Sprinting);
                }

                if (shouldCrouch)
                {
                    SetState(State.Crouching);
                }

                break;

            case State.Sprinting:
                if (!shouldSprint || moveInputValue == 0)
                {
                    SetState(moveInputValue != 0 ? State.Walking : State.Idle);
                }

                break;

            case State.Crouching:
                if (!shouldCrouch)
                {
                    SetState(moveInputValue != 0 ? State.Walking : State.Idle);
                }

                break;
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

    public bool IsCrouching()
    {
        return state == State.Crouching;
    }
}
