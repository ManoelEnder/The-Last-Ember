using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public System.Action OnPressed;
    public System.Action OnReleased;

    private int objectsOnPlate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsOnPlate++;
        OnPressed?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsOnPlate--;

        if (objectsOnPlate <= 0)
            OnReleased?.Invoke();
    }
}