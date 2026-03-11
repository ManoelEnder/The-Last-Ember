using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject doorA;
    public GameObject doorB;
    public GameObject interactionUI;

    private bool playerInRange = false;
    private bool state = false;

    void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ActivateLever();
        }
    }

    void ActivateLever()
    {
        state = !state;

        if (doorA != null)
            doorA.SetActive(!state);

        if (doorB != null)
            doorB.SetActive(state);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionUI != null)
                interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }
}