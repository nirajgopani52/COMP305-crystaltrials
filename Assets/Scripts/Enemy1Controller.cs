/* Enemy1Controller.cs
 * -------------------------------
 * Authors:
 *      - Jay Ganguli
 *      - 
 *      - 
 * 
 * Last edited: 2021-03-19
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    // controller for the bat-like enemy. This enemy has no gravity (since it flyes around) and
    // should patrol left and right within a horizontal area
    [SerializeField] private GameObject player;

    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private float speed;

    [SerializeField] private int health = 1;
    [SerializeField] private Animator anim;

    private float xOrigin; // the original x position of this enemy
    private int aiState = 1; // integer representing what the enemy is currently doing (1 = moving left, 2 = mobing right, etc)

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        xOrigin = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            return;
        }

        // check if the enemy has reached the edge of its path
        if (transform.position.x <= xOrigin - leftBound)
        {
            aiState = 2;
        }
        else if (transform.position.x >= xOrigin + rightBound)
        {
            aiState = 1;
        }

        if (aiState == 1)
        {
            rb.velocity = new Vector2(-speed, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f); // flip the enemy
        }
        else if (aiState == 2)
        {
            rb.velocity = new Vector2(speed, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Hit(int damage)
    {
        health -= damage;

        anim.SetTrigger("hit");

        if (health <= 0)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 knockback = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
            rb.AddForce((knockback.normalized + new Vector2(0f, 1f)) * 500f);
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Hit(1, transform.position, 300f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(((-leftBound + rightBound) / 2) + transform.position.x, transform.position.y - 0.5f, 0f),
            new Vector3(rightBound + leftBound, 1f, 0f));
    }
}
