
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    //public bool isChasing;
    private float attackCooldownTimer;
    private int facingDirection = -1;

    private Rigidbody2D rb;
    private Transform player;

    private EnemyState enemyState, newState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(EnemyState.Idle);
        rb = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (enemyState != EnemyState.Knockback)
        {




            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {

            }
        }
    }

    void Chase()
    {
 


        if (player.position.x > transform.position.x && facingDirection == -1 ||
                   player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }

            else if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
        
    }


    public void ChangeState(EnemyState newState)
    {
        enemyState = newState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
    }

}

public enum EnemyState
{
    Idle,
    Chasing,
    Knockback,
    Attacking
}
