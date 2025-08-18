using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI noteContent;

    private void Start()
    {
        NotesManager.Instance.OnNoteOpened += NotesManager_OnNoteOpened;
        NotesManager.Instance.OnNoteClosed += NotesManager_OnNoteClosed;

        closeButton.onClick.AddListener(() =>
        {
            NotesManager.Instance.HideNote();
        });

        Hide();
    }

    void OnDestroy()
    {
        if (NotesManager.Instance != null)
        {
            NotesManager.Instance.OnNoteOpened -= NotesManager_OnNoteOpened;
            NotesManager.Instance.OnNoteClosed -= NotesManager_OnNoteClosed;
        }
    }

    private void NotesManager_OnNoteOpened(object sender, NotesManager.OnNoteClickedEventArgs e)
    {
        noteContent.text = e.noteSO.noteContent;
        Show();
    }

    private void NotesManager_OnNoteClosed(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
