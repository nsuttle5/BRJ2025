using UnityEngine;

public class BalloonMissile : MonoBehaviour 
{
    public GameObject target;
    public float floatSpeed = 2f;
    public float wobbleAmount = 0.5f;
    public float wobbleSpeed = 2f;
    
    private float timeAlive;
    
    public void Initialize(GameObject target) {
        this.target = target;
        timeAlive = 0f;
    }
    
    void Update()
    {
        timeAlive += Time.deltaTime;
        
        // Calculate direction to target
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        
        // Add floating motion
        Vector3 wobble = new Vector3(
            Mathf.Sin(timeAlive * wobbleSpeed) * wobbleAmount,
            Mathf.Cos(timeAlive * wobbleSpeed * 0.6f) * wobbleAmount,
            0
        );
        
        // Move towards target with wobble
        transform.position += (directionToTarget * floatSpeed + wobble) * Time.deltaTime;
        
        // Optional: Rotate the balloon to face movement direction
        // float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        
        // ToDo: implement damage
        if (Vector3.Distance(transform.position, target.transform.position) <= 0.5f) {
            Destroy(gameObject);
        }
    }
}