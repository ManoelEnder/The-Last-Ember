using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public System.Action OnActivated;

    private bool isActivated;

    public void Interact()
    {
        if (isActivated) return;

        isActivated = true;
        OnActivated?.Invoke();
    }
}