using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepyCrawlerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private  int health = 2;
    [SerializeField] private float speed = 3;

    [Header("Movement")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundLookAhead;
    [SerializeField] private Transform wallCheckPos;

    [Header("Attacking")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform attackPos;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isMovingLeft = true;
    private Vector3 initialScale;
    private Vector3 flipX;
    private float attackCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", true);

        initialScale = transform.localScale;
        flipX = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            return;
        }

        if (WallCheck() || CliffCheck())
        {
            isMovingLeft = !isMovingLeft;
        }

        if (attackCooldown <= 0 && IsPlayerInRange())
        {
            rb.velocity = Vector2.zero;
            anim.SetTrigger("attack");
            attackCooldown = 1.5f;
            StartCoroutine(AttackPlayer()); // coroutine handles dealing damage to the player after waiting for the attack to "start up"
        }
        else if (attackCooldown > 0)
        {
            attackCooldown -= Time.fixedDeltaTime;
        }
        else
        {
            if (isMovingLeft)
            {
                rb.velocity = new Vector2(-speed, 0f);
                transform.localScale = flipX;
            }
            else
            {
                rb.velocity = new Vector2(speed, 0f);
                transform.localScale = initialScale;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Hit(1, transform.position, 300f);
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapCircle(wallCheckPos.position, 0.1f, whatIsGround);
    }

    private bool CliffCheck()
    {
        // returns true if there is a cliff (i.e. if there is no ground ahead)
        return !(Physics2D.OverlapCircle(groundLookAhead.position, 0.2f, whatIsGround));
    }

    private bool IsPlayerInRange()
    {
        return Physics2D.OverlapCircle(attackPos.position, 0.3f, whatIsPlayer);
    }

    // wait a brief delay after the attack starts, then check if the player is still in range before doing damage
    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        if (IsPlayerInRange())
        {
            player.GetComponent<PlayerController>().Hit(1, transform.position, 400f);
        }
        StopCoroutine(AttackPlayer());
    }

    public void Hit(int damage)
    {
        health -= damage;

        anim.SetTrigger("hit");

        if (health <= 0)
        {
            Vector2 knockback = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
            rb.AddForce((knockback.normalized + new Vector2(0f, 1f)) * 500f);
            Destroy(gameObject, 0.5f);
        }
    }
}
