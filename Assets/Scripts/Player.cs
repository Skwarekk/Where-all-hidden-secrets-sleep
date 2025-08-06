using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walking,
        Sprinting,
        Crouching
    }

    public event EventHandler OnStateChanged;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float sprintSpeedMultiplier;
    [SerializeField] private float crouchSpeedMultiplier;

    private Rigidbody2D rigidBody;
    private float currentVelocity;
    private State state;
    private float moveInputValue;

    private bool shouldSprint;
    private bool shouldCrouch;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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

    private void Update()
    {
        moveInputValue = GameInput.Instance.GetInputAxis();
        UpdateState();
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
        rigidBody.linearVelocity = new Vector2(moveInputValue * currentVelocity, rigidBody.linearVelocity.y);
    }

    private void UpdateState()
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

    private void UpdateVelocity()
    {
        switch (state)
        {
            case State.Idle:
                currentVelocity = 0;
                break;

            case State.Walking:
                currentVelocity = playerSpeed;
                break;

            case State.Sprinting:
                currentVelocity = playerSpeed * sprintSpeedMultiplier;
                break;

            case State.Crouching:
                currentVelocity = playerSpeed * crouchSpeedMultiplier;
                break;
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

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
