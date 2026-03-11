using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 6f;

    [Header("Combat")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDelay = 0.5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private Transform player;

    private int currentHealth;
    private float attackTimer;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        PlayerController p = FindFirstObjectByType<PlayerController>();
        if (p != null) player = p.transform;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0) sprite.flipX = false;
        if (direction.x < 0) sprite.flipX = true;
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;

        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;

        animator.SetTrigger("Attack");

        Invoke(nameof(DealDamage), attackDelay);
    }

    private void DealDamage()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
                pc.TakeDamage(damage);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("Die");

        Destroy(gameObject, 1.5f);
    }
}