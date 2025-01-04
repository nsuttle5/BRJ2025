using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float defaultFireRate;

    private float currentFireRate;
    private float fireRateTimer;

    private void Awake()
    {
        currentFireRate = defaultFireRate;
        fireRateTimer = currentFireRate;
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

                fireRateTimer = currentFireRate;
            }
        }
    }
}
