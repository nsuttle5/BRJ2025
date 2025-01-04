using UnityEngine;

public class StatsHandler : MonoBehaviour
{
    public static StatsHandler Instance;

    public float moveSpeedMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float damageMultiplier = 1f;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }


}
