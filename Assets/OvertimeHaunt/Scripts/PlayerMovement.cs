using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public int facingDirection = 1;
    public Rigidbody2D rb;

    public Animator anim;

    public Player_Combat player_Combat;

    private void Update()
    {
        if(Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }
    }

    // Fixed Update is called 50x per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal > 0 && transform.localScale.x <0 ||
            horizontal < 0 && transform.localScale.x >0)
        {
            Flip();
        }

        rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
