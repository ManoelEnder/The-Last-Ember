using UnityEngine;
using TMPro;

public class Box : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 2f;
    public TextMeshProUGUI interactText;

    Vector3 startPos;
    Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        interactText.enabled = false;
    }

    void Update()
    {
        float dist = Vector2.Distance(player.position, transform.position);

        if (dist <= interactDistance)
        {
            interactText.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                RespawnBox();
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }

    void RespawnBox()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.position = startPos;
        transform.rotation = Quaternion.identity;
    }
}