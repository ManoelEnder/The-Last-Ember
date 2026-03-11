using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public Transform player;
    public float distance = 0.8f;
    public int damage = 1;

    Camera cam;
    Collider2D swordCollider;
    SpriteRenderer sprite;

    bool equipped = true;
    bool attacking = false;

    void Start()
    {
        cam = Camera.main;
        swordCollider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        swordCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            equipped = !equipped;
            sprite.enabled = equipped;
        }

        if (!equipped) return;

        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;

        Vector2 dir = (mouse - player.position).normalized;

        transform.position = player.position + (Vector3)(dir * distance);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0))
        {
            attacking = true;
            swordCollider.enabled = true;
            Invoke("StopAttack", 0.15f);
        }
    }

    void StopAttack()
    {
        attacking = false;
        swordCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!attacking) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }

        if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
                boss.TakeDamage(damage);
        }
    }
}