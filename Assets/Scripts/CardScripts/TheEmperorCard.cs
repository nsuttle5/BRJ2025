using UnityEngine;
using System.Collections;

public class TheEmperorCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        statsHandler.damageMultiplier = damageMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.damageMultiplier = 1f;
        Destroy(gameObject);
    }
}
