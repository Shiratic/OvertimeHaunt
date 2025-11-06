using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 1;
    public float knockbackForce = 10f;
    public float stunTime = 0.2f;
    public float lifeSpan = 3f;

    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeSpan); // auto-destroy after X seconds
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // --- Damage player ---
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();

            if (playerHealth != null)
                playerHealth.ChangeHealth(-damage);

            if (playerMovement != null)
                playerMovement.Knockback(transform, knockbackForce, stunTime);

            Destroy(gameObject);
        }

        // --- Destroy on wall or obstacle ---
        else if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
