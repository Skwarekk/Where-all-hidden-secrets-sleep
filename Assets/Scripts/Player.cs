using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float sprintMultiplier;
    private Rigidbody2D rigidBody;
    private bool isSprinting;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameInput.Instance.OnSprintPressed += GameInput_OnSprintPressed;
        GameInput.Instance.OnSprintReleased += GameInput_OnSprintReleased;
    }

    private void GameInput_OnSprintPressed(object sender, System.EventArgs e)
    {
       isSprinting = true;
    }

    private void GameInput_OnSprintReleased(object sender, System.EventArgs e)
    {
        isSprinting = false;
    }

    private void FixedUpdate()
    {
        float velocityValue = playerSpeed;

        if (isSprinting)
        {
            velocityValue *= sprintMultiplier;
        }
            rigidBody.linearVelocityX = GameInput.Instance.GetInputAxis() * velocityValue;
    }
}
