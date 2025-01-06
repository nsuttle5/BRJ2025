using UnityEngine;

public class TheHangedManCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float damageIncrease;

    public override void UseCard()
    {
        PlayerManager.Instance.SetCurrentHealth(PlayerManager.Instance.GetCurrentHealth() - 1);
        statsHandler.permanentDamageIncrease += damageIncrease;
        Destroy(gameObject);
    }
}
