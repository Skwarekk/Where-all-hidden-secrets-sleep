using UnityEngine;

public class Note : InteractableObject
{
    [SerializeField] private NoteSO noteSO;

    public override void Interact()
    {
        if (noteSO == null)
        {
            Debug.LogError("NoteSO is not assigned in the inspector.");
        }

        NotesManager.Instance.ShowNote(noteSO);
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
