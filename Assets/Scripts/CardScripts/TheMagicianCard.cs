using System.Collections;
using UnityEngine;

public class TheMagicianCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float fireRateMultiplier;
    [SerializeField] private float damageIncreaseAmount;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        //Increase fire rate and damage for some time
        statsHandler.fireRateMultiplier = fireRateMultiplier;
        statsHandler.damageMultiplier = damageIncreaseAmount;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.fireRateMultiplier = 1f;
        statsHandler.damageMultiplier = 1f;
        Destroy(gameObject);
    }

}
