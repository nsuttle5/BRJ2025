using UnityEngine;
using System.Collections;

public class TheHermitCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float dashCooldownMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        statsHandler.jumpHeightMultiplier = jumpHeightMultiplier;
        statsHandler.dashCooldownMultiplier = dashCooldownMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.jumpHeightMultiplier = 1f;
        statsHandler.dashCooldownMultiplier = 1f;
        Destroy(gameObject);
    }
}
