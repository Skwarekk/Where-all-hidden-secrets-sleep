using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] private Loader.Scene targetScene;
    [SerializeField] private bool isDoorOpen = false;

    public override void Interact()
    {
        if (isDoorOpen)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Loader.Load(targetScene);
    }

    public void SetDoorState(bool state)
    {
        isDoorOpen = state;
    }
}
