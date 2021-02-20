using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCollectible : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float amplitude = 0.15f;
    [SerializeField] private int pointValue = 1;

    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            origin.y + (float)System.Math.Sin(Time.fixedTime * Mathf.PI * speed) * amplitude,
            0f);
    }

    public int GetPoints()
    {
        return pointValue;
    }
}
