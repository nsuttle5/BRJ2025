using System.Linq;
using UnityEngine;

public class CookelsBalloonAttack : MonoBehaviour {
    
    public Transform restingTarget; // The place where the boss will move and stay during this attack, could be a platform or something like that
    public int missileCount;
    public GameObject balloonPrefab;
    public GameObject balloonTarget; // we could hardcode this but we could also target platforms or other things in the scenario
    public float coolDown;
    
    private float currentCooldown;
    
    // ToDo: add animations
    
    void Start() {
        // when enabled teleport to resting target
        transform.position = restingTarget.position;
        currentCooldown = coolDown;
    }
    
    void Update() {
        currentCooldown -= Time.deltaTime;
        
        if (currentCooldown <= 0) {
            SpawnBalloons();
            currentCooldown = coolDown;
        }
    }
    void SpawnBalloons() {
        foreach (var i in Enumerable.Range(0, missileCount)) {
            // Spawn balloon slightly offset from the boss position
            Vector3 spawnOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0
            ).normalized * 2f;
        
            GameObject balloon = Instantiate(
                balloonPrefab,
                transform.position + spawnOffset,
                Quaternion.identity
            );
        
            // Initialize the balloon's behavior
            BalloonMissile balloonScript = balloon.GetComponent<BalloonMissile>();
            if (balloonScript != null) {
                balloonScript.Initialize(balloonTarget);
            }
        }
       
    }
}
