using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public int facingDirection = 1;
    public Rigidbody2D rb;

    public Animator anim;

    public Player_Combat player_Combat;

    public GameObject crossHair;
    

    private bool isKnockedBack;
    private float activeMoveSpeed;
    public float dashSpeed;

    public float dashLength = .5f, dashCooldown = 1f;

    private float dashCounter;
    private float dashCooldownCounter;

    private void Start()
    {
        

        activeMoveSpeed = speed;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {


        // Handle combat input
        if (Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }

        // --- Crosshair follow mouse ---
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // keep it in 2D
        crossHair.transform.position = mousePos;

        // --- Player flip towards crosshair ---
        if (mousePos.x > transform.position.x && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (mousePos.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }


    }

    // Fixed Update is called 50x per frame
    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal > 0 && transform.localScale.x < 0 ||
                horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            rb.linearVelocity = new Vector2(horizontal, vertical) * activeMoveSpeed;

            // Dash input
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (dashCooldownCounter <= 0) 
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                }
            }

            // Dash duration
            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;

                if (dashCounter <= 0)
                {
                    activeMoveSpeed = speed;
                    dashCooldownCounter = dashCooldown;
                }
            }

            // Cooldown ticking
            if (dashCooldownCounter > 0)
            {
                dashCooldownCounter -= Time.deltaTime;
            }
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force; 
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero; 
        isKnockedBack = false;
    }
}
