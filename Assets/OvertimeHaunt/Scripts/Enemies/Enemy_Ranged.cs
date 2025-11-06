using UnityEngine;

public class Enemy_Ranged : Enemy_Movement
{
    [Header("Ranged Attack Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 10f;
    public int damage = 1;
    public float knockbackForce = 10f;
    public float stunTime = 0.2f;
    public AudioClip shootSound;

    private float _lastAttackTime = -Mathf.Infinity;

    protected override void PerformAttack()
    {
        if (player == null || projectilePrefab == null || shootPoint == null)
            return;

        // ✅ Prevent multiple shots during one cooldown
        if (Time.time < _lastAttackTime + attackCooldown)
            return;

        _lastAttackTime = Time.time;

        // Face the player
        if (player.position.x > transform.position.x && facingDirection == -1 ||
            player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        anim.SetTrigger("Shoot"); // optional animation trigger

        // Spawn projectile
        GameObject bullet = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ✅ Correct usage
            Vector2 dir = (player.position - shootPoint.position).normalized;
            rb.linearVelocity = dir * projectileSpeed;

            // Optional: make projectile face direction
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            Debug.LogWarning("Projectile prefab missing Rigidbody2D!");
        }

    }
}