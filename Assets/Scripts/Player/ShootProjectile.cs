using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private StatsHandler statHandler;
    [SerializeField] private Transform firePointForward;
    [SerializeField] private Transform firePointUpward;
    [SerializeField] private Transform firePointDownward;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;

    private float fireRateTimer;

    private void Awake()
    {
        fireRateTimer = fireRate * statHandler.fireRateMultiplier;
    }

    private void Update()
    {
        fireRateTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (fireRateTimer <= 0)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Instantiate(projectilePrefab, firePointUpward.position, firePointUpward.rotation);
                }
                else if (Input.GetKey(KeyCode.S)) 
                {
                    Instantiate(projectilePrefab, firePointDownward.position, firePointDownward.rotation);
                }
                else Instantiate(projectilePrefab, firePointForward.position, firePointForward.rotation);
                //Shoot


                fireRateTimer = fireRate * statHandler.fireRateMultiplier;
            }
        }
    }
}
