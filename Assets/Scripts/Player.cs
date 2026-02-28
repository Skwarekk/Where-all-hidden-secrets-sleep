using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Player Speed Settings")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float sprintSpeedMultiplier;
    [SerializeField] private float crouchSpeedMultiplier;

    [Header("Player collider settings")]
    [SerializeField] private Vector2 standColliderSize;
    [SerializeField] private Vector2 standColliderOffset;
    [SerializeField] private Vector2 crouchColliderSize;
    [SerializeField] private Vector2 crouchColliderOffset;
    [SerializeField] private LayerMask obstacleLayerMask;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private float currentVelocity;
    private float moveInputValue;
    private bool canStandUp;

    private InteractableObject selectedInteractable;
    public static event Action<InteractableObject> OnInteractableSelected;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        PlayerStateManager.Instance.OnStateChanged += PlayerStateManager_OnStateChanged;

        GameInput.Instance.OnInteractPressed += GameInput_OnInteractPressed;
    }

    private void GameInput_OnInteractPressed(object sender, EventArgs e)
    {
        HandleInteraction();
    }

    private void OnDestroy()
    {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.OnStateChanged -= PlayerStateManager_OnStateChanged;
        }

        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnInteractPressed -= GameInput_OnInteractPressed;

        }
    }

    private void PlayerStateManager_OnStateChanged(object sender, EventArgs e)
    {
        if (PlayerStateManager.Instance.IsIdle() || PlayerStateManager.Instance.IsCrouchingIdle())
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
        else if (PlayerStateManager.Instance.IsCrouchingWalking())
        {
            currentVelocity = playerSpeed * crouchSpeedMultiplier;
        }

        if (PlayerStateManager.Instance.IsCrouching())
        {
            boxCollider.size = crouchColliderSize;
            boxCollider.offset = crouchColliderOffset;
        }
        else
        {
            boxCollider.size = standColliderSize;
            boxCollider.offset = standColliderOffset;
        }
    }

    private void Update()
    {
        moveInputValue = GameInput.Instance.GetInputAxis();

        PlayerStateManager.Instance.UpdateState(moveInputValue, canStandUp);

        SetSelectedInteractable();
    }

    private void SetSelectedInteractable()
    {
        InteractableObject newSelectedInteractable = GetClosestInteractable();

        if (newSelectedInteractable != selectedInteractable)
        {
            selectedInteractable = newSelectedInteractable;
            OnInteractableSelected?.Invoke(selectedInteractable);
        }
    }

    private void HandleInteraction()
    {
        if (selectedInteractable != null)
        {
            selectedInteractable.Interact();
        }
    }

    private bool IsCollidingWithWall()
    {
        Vector2 boxCastDirection = new Vector2(moveInputValue, 0f);
        if (boxCastDirection == Vector2.zero)
        {
            return false;
        }

        float offset = 0.01f;
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, boxCastDirection, offset, obstacleLayerMask);
    }

    private InteractableObject GetClosestInteractable()
    {
        InteractableObject closestInteractableObject = null;
        float minDistance = float.MaxValue;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();
            if (interactable != null && !interactable.Disabled())
            {
                float distance = Vector2.Distance(transform.position, interactable.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestInteractableObject = interactable;
                }
            }
        }

        return closestInteractableObject;
    }

    private void FixedUpdate()
    {
        canStandUp = !Physics2D.BoxCast(transform.position, standColliderSize, 0f, Vector2.up, 0.1f, obstacleLayerMask);

        Vector2 targetVelocity = new Vector2(moveInputValue * currentVelocity, rigidBody.linearVelocity.y);

        if (IsCollidingWithWall())
        {
            targetVelocity.x = 0;
        }
        rigidBody.linearVelocity = targetVelocity;
    }
}