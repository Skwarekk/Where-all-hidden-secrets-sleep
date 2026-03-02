using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesUI : OpenableUI
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI noteContent;
    [SerializeField] private TextMeshProUGUI noteDate;
    [SerializeField] private Image noteDrawing;

    protected override void Start()
    {
        base.Start();

        NotesManager.Instance.OnNoteOpened += NotesManager_OnNoteOpened;
        NotesManager.Instance.OnNoteClosed += NotesManager_OnNoteClosed;

        closeButton.onClick.AddListener(() =>
        {
            NotesManager.Instance.HideNote();
        });
    }

    private void OnDestroy()
    {
        if (NotesManager.Instance != null)
        {
            NotesManager.Instance.OnNoteOpened -= NotesManager_OnNoteOpened;
            NotesManager.Instance.OnNoteClosed -= NotesManager_OnNoteClosed;
        }
    }

    private void NotesManager_OnNoteOpened(object sender, NotesManager.OnNoteClickedEventArgs e)
    {
        NoteSO noteSO = e.noteSO;
        if (noteSO.isDrawing)
        {
            noteDrawing.gameObject.SetActive(true);
            noteDrawing.sprite = noteSO.drawing;
        }
        else
        {
            noteDrawing.gameObject.SetActive(false);
            noteDate.text = noteSO.withDate ? noteSO.noteDate.month + "." + noteSO.noteDate.day + "." + noteSO.noteDate.year : "";
            noteContent.text = noteSO.noteContent;
        }
        Show();
    }

    private void NotesManager_OnNoteClosed(object sender, System.EventArgs e)
    {
        Hide();
    }
}
