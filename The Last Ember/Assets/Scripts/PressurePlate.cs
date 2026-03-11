using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Door door;
    public static int platesPressed = 0;
    public static int platesRequired = 3;

    private bool pressed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!pressed && (other.CompareTag("Box") || other.CompareTag("Player")))
        {
            pressed = true;
            platesPressed++;

            if (platesPressed >= platesRequired)
                door.Open();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (pressed && (other.CompareTag("Box") || other.CompareTag("Player")))
        {
            pressed = false;
            platesPressed--;
        }
    }
}