using System.Collections;
using UnityEngine;

public class TheMagicianCard : Card
{
    [SerializeField] private float fireRateMultiplier;
    [SerializeField] private float damageIncreaseAmount;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        //Increase fire rate and damage for some time
        StatsHandler.Instance.fireRateMultiplier = fireRateMultiplier;
        StatsHandler.Instance.damageMultiplier = damageIncreaseAmount;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        StatsHandler.Instance.fireRateMultiplier = 1f;
        StatsHandler.Instance.damageMultiplier = 1f;
        Destroy(gameObject);
    }
}
