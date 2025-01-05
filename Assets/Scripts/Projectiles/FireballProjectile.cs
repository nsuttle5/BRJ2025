using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float aliveTime;
    [SerializeField] private float damage = 20f;

    private void Update()
    {
        transform.position += transform.right * projectileSpeed * Time.deltaTime;

        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Damage logic

        Destroy(gameObject);
    }
}
