using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private GameObject doorVisual;

    public void Open()
    {
        doorCollider.enabled = false;
        doorVisual.SetActive(false);
    }
}