using UnityEngine;
using UnityEngine.UI;

public class PinpadUI : OpenableUI
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMPro.TextMeshProUGUI pinDisplayText;

    protected override void Start()
    {
        base.Start();

        PinpadManager.Instance.OnPinpadOpened += PinpadManager_OnPinpadOpened;
        PinpadManager.Instance.OnPinpadClosed += PinpadManager_OnPinpadClosed;
        PinpadManager.Instance.OnPinpadButtonClicked += PinpadManager_OnPinpadButtonClicked;

        closeButton.onClick.AddListener(() =>
        {
            PinpadManager.Instance.HidePinpad();
        });
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
        pinDisplayText.text = PinpadManager.Instance.GetCurrentCode();
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
}
