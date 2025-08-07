using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float sprintSpeedMultiplier;
    [SerializeField] private float crouchSpeedMultiplier;

    private Rigidbody2D rigidBody;
    private float currentVelocity;
    private float moveInputValue;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PlayerStateManager.Instance.OnStateChanged += PlayerStateManager_OnStateChanged;
    }

    private void PlayerStateManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (PlayerStateManager.Instance.IsIdle())
        {
            currentVelocity = 0;
        }
        else if (PlayerStateManager.Instance.IsWalking())
        {
            currentVelocity = playerSpeed;
        }
        else if (PlayerStateManager.Instance.IsSprinting())
        {
            currentVelocity = playerSpeed * sprintSpeedMultiplier;
        }
        else if (PlayerStateManager.Instance.IsCrouching())
        {
            currentVelocity = playerSpeed * crouchSpeedMultiplier;
        }
    }

    private void Update()
    {
        moveInputValue = GameInput.Instance.GetInputAxis();

        PlayerStateManager.Instance.UpdateState(moveInputValue);
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocity = new Vector2(moveInputValue * currentVelocity, rigidBody.linearVelocity.y);
    }
}
