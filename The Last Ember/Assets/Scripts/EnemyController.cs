using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("AI")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRadius = 6f;

    [Header("Combat")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int contactDamage = 1;

    private Rigidbody2D rb;
    private Transform player;

    private int currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>().transform;
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.position) > detectionRadius)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (player.position - transform.position).normalized;

        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController playerController =
            collision.gameObject.GetComponent<PlayerController>();

        if (playerController == null) return;

        playerController.TakeDamage(contactDamage);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}