using UnityEngine;

public class SelectedInteractableObjectVisual : MonoBehaviour
{
    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        Player.OnInteractableSelected += Player_OnInteractableSelected;
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
            Hide();
        }
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}
