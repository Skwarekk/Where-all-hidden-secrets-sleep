using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected bool disabled;
    public abstract void Interact();

    public bool Disabled()
    {
        return disabled;
    }
}