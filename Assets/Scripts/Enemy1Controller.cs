using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    // controller for the bat-like enemy. This enemy has no gravity (since it flyes around) and
    // should patrol left and right within a horizontal area
    [SerializeField] private Transform player;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private float speed;

    private float xOrigin; // the original x position of this enemy
    private int aiState = 1; // integer representing what the enemy is currently doing (1 = moving left, 2 = mobing right, etc)

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        xOrigin = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(((-leftBound + rightBound) / 2) + transform.position.x, transform.position.y - 0.5f, 0f),
            new Vector3(rightBound + leftBound, 1f, 0f));
    }
}
