using System;
using UnityEngine;

public class PinpadManager : MonoBehaviour
{
    public event EventHandler<OnPinpadOpenedEventArgs> OnPinpadOpened;
    public event EventHandler OnPinpadButtonClicked;
    public class OnPinpadOpenedEventArgs : EventArgs
    {
        public PinPad pinPad;
    }

    public event EventHandler OnPinpadClosed;
    private PinPad currentOpenedPinpad;

    public static PinpadManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of PinpadManager!");
        }

        Instance = this;
    }

    private void Start()
    {
        PinpadButtonUI.OnPinpadButtonClicked += PinpadButtonUI_OnPinpadButtonClicked;
        PinpadButtonUI.OnPinpadClearButtonClicked += PinpadButtonUI_OnPinpadClearButtonClicked;
        PinpadButtonUI.OnPinpadSubmitButtonClicked += PinpadButtonUI_OnPinpadSubmitButtonClicked;
    }

    private void PinpadButtonUI_OnPinpadButtonClicked(object sender, PinpadButtonUI.OnPinpadButtonClickedEventArgs e)
    {
        if (currentOpenedPinpad == null)
        {
            return;
        }

        if (currentOpenedPinpad.GetCurrentCode().Length + 1 <= currentOpenedPinpad.GetPinLength())
        {
            currentOpenedPinpad.SetNewCode(currentOpenedPinpad.GetCurrentCode() + e.buttonValue);
            OnPinpadButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }

    private void PinpadButtonUI_OnPinpadClearButtonClicked(object sender, EventArgs e)
    {
        if (currentOpenedPinpad == null)
        {
            return;
        }

        currentOpenedPinpad.ClearCode();
        OnPinpadButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void PinpadButtonUI_OnPinpadSubmitButtonClicked(object sender, EventArgs e)
    {
        if (currentOpenedPinpad == null)
        {
            return;
        }

        currentOpenedPinpad.SubmitCode();
        OnPinpadButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void ShowPinpad(PinPad pinPad)
    {
        OnPinpadOpened?.Invoke(this, new OnPinpadOpenedEventArgs { pinPad = pinPad });
        GameInput.Instance.Disable();
    }

    public void HidePinpad()
    {
        OnPinpadClosed?.Invoke(this, EventArgs.Empty);
        GameInput.Instance.Enable();
    }

    public string GetCurrentCode()
    {
        return currentOpenedPinpad.GetCurrentCode();
    }

    public void SetCurrentOpenedPinpad(PinPad pinPad)
    {
        currentOpenedPinpad = pinPad;
    }
}
