
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    //public bool isChasing;
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
        if (enemyState != EnemyState.Knockback)
        {




            if (enemyState == EnemyState.Chasing)
            {
                if (player.position.x > transform.position.x && facingDirection == -1 ||
                    player.position.x < transform.position.x && facingDirection == 1)
                {
                    Flip();
                }
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(player == null)
            {
                player = collision.transform;
            }
            
            //isChasing = true;
            ChangeState(EnemyState.Chasing);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero;
            //isChasing = false;
            ChangeState(EnemyState.Idle);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        enemyState = newState;
    }

    public enum EnemyState
    {
        Idle,
        Chasing,
        Knockback
    }
}
