using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameObject visualGameObject;
    [SerializeField] private float openPositionY = 1.2f;
    private BoxCollider2D boxCollider2D;
    private bool isOpen = false;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Open()
    {
        isOpen = true;
        visualGameObject.transform.position = new Vector3(
            visualGameObject.transform.position.x,
            visualGameObject.transform.position.y + openPositionY,
            visualGameObject.transform.position.z
    );
        boxCollider2D.enabled = false;
    }

    public void Close()
    {
        isOpen = false;
        visualGameObject.transform.position = new Vector3(
            visualGameObject.transform.position.x,
            visualGameObject.transform.position.y - openPositionY,
            visualGameObject.transform.position.z
        );
        boxCollider2D.enabled = true;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
