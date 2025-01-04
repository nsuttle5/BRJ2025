using UnityEngine;
using System.Collections;

public class TheChariotCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float dashDistanceMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        statsHandler.dashDistanceMultiplier = dashDistanceMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.dashDistanceMultiplier = 1f;
        Destroy(gameObject);
    }
}
