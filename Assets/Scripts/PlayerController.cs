/* PlayerController.cs
 * -------------------------------
 * Authors:
 *      - Jay Ganguli
 *      - 
 *      - 
 * 
 * Last edited: 2021-02-18
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 500f;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask whatIsGround;
    // the animator is in a child component to account for the sprite offset when flipping
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        isGrounded = GroundCheck();

        if (horizontalMove == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else if (horizontalMove > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // face right
            anim.SetBool("isRunning", true);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // face left
            anim.SetBool("isRunning", true);
        }



        // jump code
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);

            if (Input.GetAxis("Jump") > 0)
            {
                rb.AddForce(new Vector2(0.0f, jumpForce));
                isGrounded = false;
                anim.SetBool("isJumping", true);
            }
        }

        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, whatIsGround);
    }
}
