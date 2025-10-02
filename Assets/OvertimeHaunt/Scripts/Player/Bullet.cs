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

    private Vector2 direction;

    // ✅ Called by Player_Gun right after Instantiate
    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        rb.linearVelocity = direction * speed;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            var enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
                enemyHealth.ChangeHealth(-damage);

            var enemyKnockback = collision.gameObject.GetComponent<Enemy_Knockback>();
            if (enemyKnockback != null)
                enemyKnockback.Knockback(transform, knockbackForce, knockbackTime, stunTime);

            Destroy(gameObject);
        }
    }
}
