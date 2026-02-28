using System;
using UnityEngine;
using UnityEngine.UI;

public class PinpadButtonUI : MonoBehaviour
{
    public static event EventHandler<OnPinpadButtonClickedEventArgs> OnPinpadButtonClicked;
    public static event EventHandler OnPinpadClearButtonClicked;
    public static event EventHandler OnPinpadSubmitButtonClicked;
    public class OnPinpadButtonClickedEventArgs : EventArgs
    {
        public string buttonValue;
    }

    private enum ButtonType
    {
        Digit,
        Clear,
        Submit
    }

    [SerializeField] private ButtonType buttonType;
    [SerializeField] private string buttonValue;
    [SerializeField] private TMPro.TextMeshProUGUI buttonText;
    [SerializeField] private Color buttonColor;

    private Button button;
    private Image image;
    private delegate void ButtonClickAction();

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        ButtonClickAction buttonClickAction = null;

        switch (buttonType)
        {
            case ButtonType.Digit:
                buttonText.text = buttonValue;
                buttonClickAction = () =>
                {
                    OnPinpadButtonClicked?.Invoke(this, new OnPinpadButtonClickedEventArgs { buttonValue = buttonValue });
                };
                break;
            case ButtonType.Clear:
                buttonClickAction = () =>
                {
                    OnPinpadClearButtonClicked?.Invoke(this, EventArgs.Empty);
                };
                buttonText.text = "X";
                break;
            case ButtonType.Submit:
                buttonClickAction = () =>
                {
                    OnPinpadSubmitButtonClicked?.Invoke(this, EventArgs.Empty);
                };
                buttonText.text = "S";
                break;
        }

        button.onClick.AddListener(() =>
        {
            buttonClickAction();
        });

        image.color = buttonColor;
    }
}
