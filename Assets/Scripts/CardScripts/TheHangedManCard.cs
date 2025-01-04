using UnityEngine;

public class TheHangedManCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float damageMultiplier;

    public override void UseCard()
    {
        PlayerManager.Instance.Heal(-1);
        statsHandler.damageMultiplier = damageMultiplier;
        Destroy(gameObject);
    }
}
