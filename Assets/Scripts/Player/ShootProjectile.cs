using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;

    private float fireRateTimer;

    private void Awake()
    {
        fireRateTimer = fireRate * StatsHandler.Instance.fireRateMultiplier;
    }

    private void Update()
    {
        fireRateTimer -= Time.deltaTime;
        Debug.Log(fireRateTimer);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (fireRateTimer <= 0)
            {
                //Shoot
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

                fireRateTimer = fireRate * StatsHandler.Instance.fireRateMultiplier;
            }
        }
    }
}
