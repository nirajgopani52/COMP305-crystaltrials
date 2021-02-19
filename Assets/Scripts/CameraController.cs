using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Range(0f, 10f)][SerializeField] private float cameraOffsetX = 2f;
    [Range(0f, 10f)][SerializeField] private float cameraOffsetY = 2f;

    // limits on the camera positions (so we don't see past the edge of the level)
    [SerializeField] private Transform leftBound;

    // Update is called once per frame
    void Update()
    {
        if (player.position.x < transform.position.x - cameraOffsetX)
        {
            transform.position = new Vector3(player.position.x + cameraOffsetX, transform.position.y, transform.position.z);
        }
        if (player.position.x > transform.position.x + cameraOffsetX)
        {
            transform.position = new Vector3(player.position.x - cameraOffsetX, transform.position.y, transform.position.z);
        }

        if (player.position.y < transform.position.y - cameraOffsetY)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + cameraOffsetY, transform.position.z);
        }
        if (player.position.y > transform.position.y + cameraOffsetY)
        {
            transform.position = new Vector3(transform.position.x, player.position.y - cameraOffsetY, transform.position.z);
        }

        // clamp camera position to within bounds
        if (transform.position.x < leftBound.position.x)
        {
            transform.position = new Vector3(leftBound.position.x, transform.position.y, transform.position.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(cameraOffsetX * 2, cameraOffsetY * 2, 0.0f));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector2(leftBound.position.x, leftBound.position.y + 10f),
            new Vector2(leftBound.position.x, leftBound.position.y - 10f));
    }
}
