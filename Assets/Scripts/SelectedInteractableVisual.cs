using UnityEngine;

public class selectedInteractableObjectVisual : MonoBehaviour
{
    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private GameObject iconImage;

    private bool isShown = false;

    private void Start()
    {
        Player.OnInteractableSelected += Player_OnInteractableSelected;

        Hide();
    }

    private void OnDestroy()
    {
        Player.OnInteractableSelected -= Player_OnInteractableSelected;
    }

    private void Player_OnInteractableSelected(InteractableObject selectedInteractable)
    {
        if (selectedInteractable == interactableObject)
        {
            Show();
        }
        else
        {
            if (isShown)
            {
                Hide();
            }
        }
    }

    private void Show()
    {
        iconImage.SetActive(true);
        isShown = true;
    }

    private void Hide()
    {
        iconImage.SetActive(false);
        isShown = false;
    }
}
