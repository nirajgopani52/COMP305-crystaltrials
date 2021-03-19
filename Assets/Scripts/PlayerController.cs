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
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 500f;
    [SerializeField] private float groundCheckRadius = 0.15f;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float bounceForce = 500f;
    [SerializeField] private float bounceCheckHeight = 0.2f;
    [SerializeField] private LayerMask whatIsEnemy;
    private float enemyBounceFrames = 0; // number of frames after bouncing off an enemy where the player can input a jump to gain extra height

    // the animator is in a child component to account for the sprite offset when flipping
    [SerializeField] private Animator anim;
    [SerializeField] private Text scoreText;
    [SerializeField] private HealthBar hb;

    private Rigidbody2D rb;
    private bool isGrounded;

    private int score;
    private float hitstun = 0f; // when the player gets hit and has hitstun, they cannot move

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    } 

    private void FixedUpdate()
    {
        if (hitstun > 0)
        {
            hitstun -= Time.fixedDeltaTime;
        }
        else
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
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(new Vector2(0.0f, jumpForce));
                    isGrounded = false;
                    anim.SetBool("isJumping", true);
                }
            }
            else if (enemyBounceFrames > 0)
            {
                enemyBounceFrames--;

                if (Input.GetAxis("Jump") > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(new Vector2(0f, bounceForce));
                    enemyBounceFrames = 0;
                }
            }
            else if (rb.velocity.y < 0 && EnemyBounceCheck())
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0.0f, bounceForce / 2));
                anim.SetTrigger("bounce");
                // set an amount of time where the player can boost their jump
                enemyBounceFrames = 3;
            }

            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            Hit(1, collision.GetContact(0).point, 400);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gem")
        {
            score += collision.gameObject.GetComponent<FloatingCollectible>().GetPoints();
            scoreText.text = score.ToString();
            Destroy(collision.gameObject);
        }
    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, whatIsGround);
    }

    private bool EnemyBounceCheck()
    {
        // might make it a seperate width value later
        Collider2D collider = Physics2D.OverlapBox(groundCheckPos.position, new Vector2(0.5f, bounceCheckHeight), 0f, whatIsEnemy);

        if (collider)
        {
            Staggerable enemy = collider.gameObject.GetComponent<Staggerable>();

            if (enemy)
            {
                enemy.Hit();
            }
        }

        return collider;
    }

    public void Hit(int damage)
    {
        hb.Hit(damage);
        anim.SetTrigger("hit");
    }

    public void Hit(int damage, Vector2 damageSource, float knockbackForce)
    {
        if (hitstun > 0 || enemyBounceFrames > 0)
        {
            return; // if Gino is already being hit by something, don't hit them again
        }

        hb.Hit(damage);
        anim.SetTrigger("hit");
        Vector2 knockbackDirection = new Vector2(transform.position.x - damageSource.x, transform.position.y - damageSource.y).normalized;
        // the new vector2 in here is a constant amount of vertical knockup to the player always gets bumped slightly upwards
        rb.velocity = Vector2.zero;
        rb.AddForce((knockbackDirection + new Vector2(0f, 1f)) * knockbackForce);

        hitstun = 0.2f;
        enemyBounceFrames = 0; // prevent player from jumping out of hitstun if both the bounce hitbox and damage hitbox trigger at the same time
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundCheckPos.position, new Vector3(0.5f, bounceCheckHeight, 0f));
    }
}
