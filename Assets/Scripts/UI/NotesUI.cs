using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesUI : MonoBehaviour
{
    private const string IN = "In";
    private const string OUT = "Out";

    [Header("References")]
    [Space(10)]
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI noteContent;
    [SerializeField] private TextMeshProUGUI noteDate;
    [SerializeField] private Image noteDrawing;

    [Space(20)]
    [Header("Animation settings")]
    [Space(10)]
    [SerializeField] private float outAnimationDuration = 0.5f;

    private Animator animator;

    private float outAnimatintimer = 0;
    private bool isPlayingOutAnimation = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        NotesManager.Instance.OnNoteOpened += NotesManager_OnNoteOpened;
        NotesManager.Instance.OnNoteClosed += NotesManager_OnNoteClosed;

        closeButton.onClick.AddListener(() =>
        {
            NotesManager.Instance.HideNote();
        });

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayingOutAnimation)
        {
            outAnimatintimer += Time.deltaTime;
            if (outAnimatintimer >= outAnimationDuration)
            {
                gameObject.SetActive(false);
                isPlayingOutAnimation = false;
                outAnimatintimer = 0;
            }
        }
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

    private void Show()
    {
        gameObject.SetActive(true);
        animator.SetTrigger(IN);
    }

    private void Hide()
    {
        animator.SetTrigger(OUT);
        isPlayingOutAnimation = true;
    }
}
