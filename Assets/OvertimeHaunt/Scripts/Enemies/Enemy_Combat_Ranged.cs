using UnityEngine;

public class Enemy_Combat_Ranged : MonoBehaviour
{
    [Header("Ranged Attack Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 10f;
    public int damage = 1;
    public float knockbackForce = 10f;
    public float stunTime = 0.2f;
    public LayerMask playerLayer;
    public AudioClip shootSound;

    public void Attack()
    {
        if (shootPoint == null || projectilePrefab == null) return;

        // Face direction based on scale
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        GameObject bullet = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(direction * projectileSpeed, 0f);

        // Set up the projectile damage info
        EnemyProjectile projectile = bullet.GetComponent<EnemyProjectile>();
        if (projectile != null)
        {
            projectile.damage = damage;
            projectile.knockbackForce = knockbackForce;
            projectile.stunTime = stunTime;
            projectile.playerLayer = playerLayer;
        }

        // Optional sound
        if (shootSound)
            AudioHelper.PlayClip2D(shootSound, 0.3f);
    }
}
