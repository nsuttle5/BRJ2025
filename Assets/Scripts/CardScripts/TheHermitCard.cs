using UnityEngine;
using System.Collections;

public class TheHermitCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float dashCooldownMultiplier;
    [SerializeField] private float fireRateMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        statsHandler.fireRateMultiplier = fireRateMultiplier;
        statsHandler.dashCooldownMultiplier = dashCooldownMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.fireRateMultiplier = 1f;
        statsHandler.dashCooldownMultiplier = 1f;
        Destroy(gameObject);
    }
}
