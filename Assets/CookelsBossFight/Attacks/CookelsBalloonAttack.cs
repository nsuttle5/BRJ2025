using System.Linq;
using UnityEngine;

public class CookelsBalloonAttack : MonoBehaviour {
    
    public Transform restingTarget; // The place where the boss will move and stay during this attack, could be a platform or something like that
    public int missileCount;
    public GameObject balloonPrefab;
    public GameObject balloonTarget; // we could hardcode this but we could also target platforms or other things in the scenario
    public float coolDown;
    public float initialCooldown = 1f; // The time to wait before triggering the initial attack
    
    private float currentCooldown;
    private Transform previousPosition; // the position the boss had before transitioning into the balloon attack
    private bool isEnabled;
    
    // ToDo: add animations

    public void Enable() {
        if (isEnabled) return;
        
        previousPosition = transform; // store the current position so that we can teleport back when disabling this attack
        transform.position = restingTarget.position;
        currentCooldown = initialCooldown; // wait a bit before starting to spawn balloons
        isEnabled = true;
    }
    
    public void Disable() {
        transform.position = previousPosition.position;
        isEnabled = false;
        currentCooldown = 0;
    }
    
    void Update() {
        if (!isEnabled) return;
        
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
