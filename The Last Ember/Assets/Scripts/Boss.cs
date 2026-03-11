using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public int maxHealth = 10;
    int currentHealth;

    public float speed = 2f;
    public Transform player;

    public GameObject victoryPanel;

    SpriteRenderer sr;
    Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            if (victoryPanel != null)
                victoryPanel.SetActive(true);

            Destroy(gameObject);
        }
    }

    IEnumerator DamageFlash()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        sr.color = originalColor;
    }
}