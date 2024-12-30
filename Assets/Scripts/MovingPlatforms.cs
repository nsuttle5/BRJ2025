using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [Header("Platform Settings")]
    public Transform pointA; // First point the platform moves to
    public Transform pointB; // Second point the platform moves to
    public float speed = 2f; // Speed of the platform's movement

    private Transform targetPoint; // The current target point
    private Vector3 previousPosition; // The platform's position in the previous frame

    private void Start()
    {
        // Start by moving towards pointA
        targetPoint = pointA;
        previousPosition = transform.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Move the platform towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Check if the platform has reached the target point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Switch to the other target point
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }

        // Update the platform's previous position
        previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw lines to visualize the movement path
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
