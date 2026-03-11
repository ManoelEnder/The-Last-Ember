using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
                player.Heal(healAmount);

            Destroy(gameObject);
        }
    }
}