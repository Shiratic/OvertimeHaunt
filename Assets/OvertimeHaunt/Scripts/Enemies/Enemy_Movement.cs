using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    public float roamSpeed = 2f;
    public float roamRadius = 5f;
    public float idleTime = 2f;

    [Header("Combat Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float playerDetectRange = 5f;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    public float wallCheckDistance = 0.5f;
    public LayerMask obstacleLayer;
    private float stuckTimer;
    private Vector2 lastPosition;

    private float attackCooldownTimer;
    private float idleTimer;
    private int facingDirection = -1;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    private Vector2 roamTarget;

    private EnemyState enemyState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PickNewRoamPosition();
        lastPosition = transform.position;
        ChangeState(EnemyState.Roaming);
    }

    void Update()
    {
        CheckForPlayer();

        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        if (enemyState == EnemyState.Knockback)
            return;

        switch (enemyState)
        {
            case EnemyState.Roaming:
                Roam();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Attacking:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    bool DetectWalls()
    {
        Vector2 direction = facingDirection == 1 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, obstacleLayer);
        return hit.collider != null;
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            else if (distanceToPlayer > attackRange)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            // 👇 when no player is detected, always return to roaming
            if (enemyState != EnemyState.Roaming)
            {
                PickNewRoamPosition();
                ChangeState(EnemyState.Roaming);
            }
        }
    }

    void Roam()
    {
        // Check for walls
        if (DetectWalls())
        {
            // Pick a new roam direction away from the wall
            PickNewRoamPosition();
            Flip();
            return;
        }

        // Check if reached target
        if (Vector2.Distance(transform.position, roamTarget) < 0.2f)
        {
            rb.linearVelocity = Vector2.zero;

            if (idleTimer <= 0)
            {
                PickNewRoamPosition();
                idleTimer = idleTime;
            }
            else
            {
                idleTimer -= Time.deltaTime;
            }
        }
        else
        {
            Vector2 direction = (roamTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * roamSpeed;

            // Flip direction
            if ((direction.x > 0 && facingDirection == -1) || (direction.x < 0 && facingDirection == 1))
                Flip();
        }

        // Detect if stuck (not moving for too long)
        if (Vector2.Distance(transform.position, lastPosition) < 0.05f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 1f)
            {
                PickNewRoamPosition();
                stuckTimer = 0;
            }
        }
        else
        {
            stuckTimer = 0;
        }

        lastPosition = transform.position;
    }

    void Chase()
    {
        if (player == null)
        {
            ChangeState(EnemyState.Roaming);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && attackCooldownTimer <= 0)
        {
            attackCooldownTimer = attackCooldown;
            ChangeState(EnemyState.Attacking);
            return;
        }

        if (player.position.x > transform.position.x && facingDirection == -1 ||
            player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void PickNewRoamPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle * roamRadius;
        roamTarget = (Vector2)transform.position + randomDirection;
    }

    void Flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ChangeState(EnemyState newState)
    {
        if (enemyState == newState) return;

        // Exit old animation states
        anim.SetBool("isIdle", false);
        anim.SetBool("isChasing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isChasing", false);

        enemyState = newState;

        // Enter new animation state
        switch (enemyState)
        {
            case EnemyState.Roaming:
                anim.SetBool("isChasing", true);
                break;
            case EnemyState.Chasing:
                anim.SetBool("isChasing", true);
                break;
            case EnemyState.Attacking:
                anim.SetBool("isAttacking", true);
                break;
            case EnemyState.Idle:
                anim.SetBool("isIdle", true);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
        Gizmos.color = Color.blue;
        Vector2 direction = facingDirection == 1 ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * wallCheckDistance);
    }
}


public enum EnemyState
{
    Idle,
    Roaming,
    Chasing,
    Knockback,
    Attacking
}
