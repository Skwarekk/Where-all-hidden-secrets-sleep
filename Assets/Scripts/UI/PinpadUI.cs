using UnityEngine;
using UnityEngine.UI;

public class PinpadUI : MonoBehaviour
{
    private const string IN = "In";
    private const string OUT = "Out";

    [SerializeField] private Button closeButton;
    [SerializeField] private TMPro.TextMeshProUGUI pinDisplayText;
    [SerializeField] private float outAnimationDuration = 0.5f;
    private Animator animator;
    private float outAnimatintimer = 0;
    private bool isPlayingOutAnimation = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        PinpadManager.Instance.OnPinpadOpened += PinpadManager_OnPinpadOpened;
        PinpadManager.Instance.OnPinpadClosed += PinpadManager_OnPinpadClosed;
        PinpadManager.Instance.OnPinpadButtonClicked += PinpadManager_OnPinpadButtonClicked;

        closeButton.onClick.AddListener(() =>
        {
            PinpadManager.Instance.HidePinpad();
        });

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (PinpadManager.Instance != null)
        {
            PinpadManager.Instance.OnPinpadOpened -= PinpadManager_OnPinpadOpened;
            PinpadManager.Instance.OnPinpadClosed -= PinpadManager_OnPinpadClosed;
            PinpadManager.Instance.OnPinpadButtonClicked -= PinpadManager_OnPinpadButtonClicked;
        }
    }

    private void PinpadManager_OnPinpadOpened(object sender, PinpadManager.OnPinpadOpenedEventArgs e)
    {
        PinpadManager.Instance.SetCurrentOpenedPinpad(e.pinPad);
        Show();
    }

    private void PinpadManager_OnPinpadClosed(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void PinpadManager_OnPinpadButtonClicked(object sender, System.EventArgs e)
    {
        pinDisplayText.text = PinpadManager.Instance.GetCurrentCode();
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

    private void Show()
    {
        gameObject.SetActive(true);
        pinDisplayText.text = PinpadManager.Instance.GetCurrentCode();
        animator.SetTrigger(IN);
    }

    private void Hide()
    {
        animator.SetTrigger(OUT);
        isPlayingOutAnimation = true;
    }
}
