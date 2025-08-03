using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocityX = GameInput.Instance.GetInputAxis() * playerSpeed * Time.fixedDeltaTime;
    }
}
