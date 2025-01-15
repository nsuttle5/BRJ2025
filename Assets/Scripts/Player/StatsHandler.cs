using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StatsHandler")]
public class StatsHandler : ScriptableObject
{
    public float moveSpeedMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float dashDistanceMultiplier = 1f;
    public float jumpHeightMultiplier = 1f;
    public float dashCooldownMultiplier = 1f;
    public float gravityMultiplier = 1f;
    public float permanentDamageIncrease = 1f;
    
}
