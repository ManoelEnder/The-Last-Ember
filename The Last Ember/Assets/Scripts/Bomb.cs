using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float delay = 2f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask destructibleLayer;

    private void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            destructibleLayer
        );

        foreach (var hit in hits)
            Destroy(hit.gameObject);

        Destroy(gameObject);
    }
}