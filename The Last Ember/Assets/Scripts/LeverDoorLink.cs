using UnityEngine;

public class LeverDoorLink : MonoBehaviour
{
    [SerializeField] private Lever lever;
    [SerializeField] private Door door;

    private void Awake()
    {
        lever.OnActivated += door.Open;
    }
}