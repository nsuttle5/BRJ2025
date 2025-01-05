using UnityEngine;

public class TheHangedManCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float damageIncrease;

    public override void UseCard()
    {
        PlayerManager.Instance.currentHealth--;
        statsHandler.permanentDamageIncrease += damageIncrease;
        Destroy(gameObject);
    }
}
