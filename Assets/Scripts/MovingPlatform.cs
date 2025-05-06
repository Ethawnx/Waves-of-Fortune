using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class MovingPlatform : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 1.0f;
    private Vector3 destination;

    void Start()
    {
        destination = pointB;
    }

    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, destination, step);

        // Check if the position of the platform and destination are approximately equal.
        if (Vector3.Distance(transform.position, destination) < 0.001f)
        {
            // If the platform is at pointB, set the destination to pointA, and vice versa.
            destination = destination == pointA ? pointB : pointA;
        }
    }
}