using UnityEngine;
using System.Collections;

public class TheEmpressCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        statsHandler.moveSpeedMultiplier = moveSpeedMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.moveSpeedMultiplier = 1f;
        Destroy(gameObject);
    }
}
