using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Combat")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private LayerMask interactLayer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerabilityTime = 0.5f;

    [Header("UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject gameOverPanel;

    private Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;

    private int currentHealth;
    private bool isInvulnerable;
    private float invulnerabilityTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
        HandleInteraction();
        HandleInvulnerability();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void HandleMovement()
    {
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (movement != Vector2.zero)
            lastDirection = movement;
    }

    private void HandleAttack()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            lastDirection,
            attackRange,
            enemyLayer
        );

        if (hit.collider == null) return;

        EnemyController enemy = hit.collider.GetComponent<EnemyController>();
        if (enemy != null)
            enemy.TakeDamage(attackDamage);
    }

    private void HandleInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            lastDirection,
            interactRange,
            interactLayer
        );

        if (hit.collider == null) return;

        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        interactable?.Interact();
    }

    private void HandleInvulnerability()
    {
        if (!isInvulnerable) return;

        invulnerabilityTimer -= Time.deltaTime;

        if (invulnerabilityTimer <= 0f)
            isInvulnerable = false;
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();

        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        enabled = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}