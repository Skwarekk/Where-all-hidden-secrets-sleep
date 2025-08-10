using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string IS_SPRINTING = "IsSprinting";
    private const string IS_CROUCHING = "IsCrouching";
    private const string IS_MOVING = "IsMoving";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PlayerStateManager.Instance.OnStateChanged += PlayerStateManager_OnStateChanged;
    }

    void OnDestroy()
    {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.OnStateChanged -= PlayerStateManager_OnStateChanged;
        }
    }

    private void Update()
    {
        float moveInputValue = PlayerStateManager.Instance.GetMoveInputValue();
        if (moveInputValue > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInputValue < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void PlayerStateManager_OnStateChanged(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_SPRINTING, PlayerStateManager.Instance.IsSprinting());
        animator.SetBool(IS_CROUCHING, PlayerStateManager.Instance.IsCrouching());
        animator.SetBool(IS_MOVING, PlayerStateManager.Instance.IsMoving());
    }
}
