using UnityEngine;
using System.Collections;

public class TheTowerCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float jumpHeightMultiplier = 1f;
    [SerializeField] private float buffDuration = 5f;

    public override void UseCard()
    {
        statsHandler.jumpHeightMultiplier = jumpHeightMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.jumpHeightMultiplier = 1f;
        Destroy(gameObject);
    }
}
