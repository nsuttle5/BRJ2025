using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float aliveTime;
    [SerializeField] private float damage;

    private void Awake()
    {
        damage = damage * statsHandler.damageMultiplier * statsHandler.permanentDamageIncrease;
    }

    private void Update()
    {
        transform.position += transform.right * projectileSpeed * Time.deltaTime;
        aliveTime -= Time.deltaTime;

        if (aliveTime <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Give damage to enemy

        Destroy(gameObject);
    }
}
