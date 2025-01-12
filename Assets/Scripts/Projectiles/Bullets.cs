using UnityEngine;

public class Bullets : MonoBehaviour, IObjectPool
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float aliveTime;
    [SerializeField] private float baseDamage;

    private float damage;
    private float aliveTimer;

    private void Update()
    {
        transform.position += transform.right * projectileSpeed * Time.deltaTime;
        aliveTimer -= Time.deltaTime;

        if (aliveTimer <= 0) BackToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Do damage;

        BackToPool();
    }

    private void BackToPool()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    public void SpawnObject()
    {
        damage = (baseDamage * statsHandler.damageMultiplier) + statsHandler.permanentDamageIncrease;
        aliveTimer = aliveTime;
    }
}
