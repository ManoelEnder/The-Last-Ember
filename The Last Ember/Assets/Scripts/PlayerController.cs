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

    [SerializeField] private GameObject sword;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float attackCooldown = 0.4f;

    private float attackTimer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;

    private int currentHealth;
    private bool isInvulnerable;
    private float invulnerabilityTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        UpdateAnimation();
        attackTimer -= Time.deltaTime;
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

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
    }

    private void HandleAttack()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;

        sword.SetActive(true);

        Invoke(nameof(HideSword), attackDuration);
    }

    private void HideSword()
    {
        sword.SetActive(false);
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