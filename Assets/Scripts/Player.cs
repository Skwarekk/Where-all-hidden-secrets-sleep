using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walking,
        Running
    }

    public event EventHandler OnStateChanged;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float sprintMultiplier;

    private Rigidbody2D rigidBody;
    private float currentVelocity;
    private State state;
    private float moveInputValue;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        SetState(State.Idle);

    }

    private void Start()
    {
        GameInput.Instance.OnSprintPressed += GameInput_OnSprintPressed;
        GameInput.Instance.OnSprintReleased += GameInput_OnSprintReleased;
    }

    private void GameInput_OnSprintPressed(object sender, System.EventArgs e)
    {
        if (IsWalking())
        {
            SetState(State.Running);

        }
    }

    private void GameInput_OnSprintReleased(object sender, System.EventArgs e)
    {
        if (IsRunning())
        {
            SetState(State.Walking);
        }
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

    private void OnDestroy()
    {
        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnSprintPressed -= GameInput_OnSprintPressed;
            GameInput.Instance.OnSprintReleased -= GameInput_OnSprintReleased;
        }
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
                break;

            case State.Walking:
                if (moveInputValue == 0)
                {
                    SetState(State.Idle);
                }
                break;

            case State.Running:
                if (moveInputValue == 0)
                {
                    SetState(State.Idle);
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

            case State.Running:
                currentVelocity = playerSpeed * sprintMultiplier;
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

    public bool IsRunning()
    {
        return state == State.Running;
    }
}
