using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject door;
    public GameObject interactionUI;

    private bool playerInRange = false;
    private bool activated = false;

    void Start()
    {
        interactionUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !activated && Input.GetKeyDown(KeyCode.E))
        {
            ActivateLever();
        }
    }

    void ActivateLever()
    {
        activated = true;
        interactionUI.SetActive(false);

        if (door != null)
        {
            door.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            playerInRange = true;
            interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionUI.SetActive(false);
        }
    }
}