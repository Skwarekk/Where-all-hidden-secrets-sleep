using System;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    public event EventHandler<OnNoteClickedEventArgs> OnNoteOpened;
    public event EventHandler OnNoteClosed;
    public class OnNoteClickedEventArgs : EventArgs
    {
        public NoteSO noteSO;
    }
    public static NotesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of NotesManager!");
        }

        Instance = this;
    }

    public void ShowNote(NoteSO noteSO)
    {
        OnNoteOpened?.Invoke(this, new OnNoteClickedEventArgs { noteSO = noteSO });
        GameInput.Instance.Disable();
    }

    public void HideNote()
    {
        OnNoteClosed?.Invoke(this, EventArgs.Empty);
        GameInput.Instance.Enable();
    }
}
