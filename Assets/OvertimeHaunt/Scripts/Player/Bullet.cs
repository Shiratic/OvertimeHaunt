using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float lifeSpawn = 2f;
    public float speed = 10f;

    public int damage;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;

    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;

    private Vector2 direction;

    // ✅ Called by Player_Gun right after Instantiate
    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        rb.linearVelocity = direction * speed; // ← fixed field name
        RotateBullet();
        Destroy(gameObject, lifeSpawn);
    }

    private void RotateBullet()
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ✅ Damage enemies
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            var enemyHealth = collision.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
                enemyHealth.ChangeHealth(-damage);

            var enemyKnockback = collision.GetComponent<Enemy_Knockback>();
            if (enemyKnockback != null)
                enemyKnockback.Knockback(transform, knockbackForce, knockbackTime, stunTime);

            Destroy(gameObject);
        }
        // ✅ Destroy when hitting obstacles
        else if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
