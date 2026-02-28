using UnityEngine;

public class PinPad : InteractableObject
{
    [SerializeField] private Gate linkedGate;
    [SerializeField] private string correctPin;

    private string enteredCode = "";

    public override void Interact()
    {
        if (!linkedGate.IsOpen() && !disabled)
        {
            PinpadManager.Instance.ShowPinpad(this);
        }
    }

    public void SubmitCode()
    {
        if (enteredCode == correctPin)
        {
            PinpadManager.Instance.HidePinpad();
            disabled = true;
            linkedGate.Open();
        }
        enteredCode = "";
    }

    public void ClearCode()
    {
        enteredCode = "";
    }

    public string GetCurrentCode()
    {
        return enteredCode;
    }

    public void SetNewCode(string newCode)
    {
        enteredCode = newCode;
    }

    public int GetPinLength()
    {
        return correctPin.Length;
    }
}
