using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private StatsHandler statHandler;
    [SerializeField] private Transform firePoint;
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
                //Shoot
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

                fireRateTimer = fireRate * statHandler.fireRateMultiplier;
            }
        }
    }
}
