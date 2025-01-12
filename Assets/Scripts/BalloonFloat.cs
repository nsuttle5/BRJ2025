using UnityEngine;

public class BalloonFloat : MonoBehaviour
{
    public float buoyancyForce = 5f; // Upward force to simulate buoyancy
    public float gravityFactor = 1f; // Factor to simulate the downward pull
    public Transform anchorPoint; // Optional: Attach the balloon to a point
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (anchorPoint == null)
        {
            // Apply upward buoyancy force
            Vector3 buoyancy = Vector3.up * buoyancyForce;
            rb.AddForce(buoyancy, ForceMode.Force);

            // Simulate reduced gravity effect
            Vector3 reducedGravity = Physics.gravity * -gravityFactor;
            rb.AddForce(reducedGravity, ForceMode.Acceleration);
        }
        else
        {
            // Keep the balloon at the anchor point
            rb.linearVelocity = Vector3.zero;
            rb.position = anchorPoint.position;
        }
    }
}
