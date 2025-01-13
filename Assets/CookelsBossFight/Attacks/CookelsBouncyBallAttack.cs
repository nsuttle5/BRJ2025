using UnityEngine;

public class CookelsBouncyBallAttack : MonoBehaviour
{
    public Transform restingTarget; // The place where the boss will move and stay during this attack, could be a platform or something like that
    public GameObject ballPrefab;
    public GameObject stageCenter; 
    
    private Transform previousPosition; // the position the boss had before transitioning into the balloon attack
    private GameObject ball;
    private bool isEnabled;
    
    public void Enable() {
        if (isEnabled) return;
        
        previousPosition = transform; // store the current position so that we can teleport back when disabling this attack
        transform.position = restingTarget.position;
        
        // Spawn the ball
        Vector3 spawnOffset = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0
        ).normalized * 2f;
        
        ball = Instantiate(
            ballPrefab,
            transform.position + spawnOffset,
            Quaternion.identity
        );
        ball.GetComponent<BouncingBall>().Initialize(stageCenter);

        isEnabled = true;
    }

    public void Disable() {
        transform.position = previousPosition.position;
        Destroy(ball);
        isEnabled = false;
    }
}
