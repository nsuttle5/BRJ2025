using UnityEngine;
using System.Collections;

public class TheMoon : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        statsHandler.gravityMultiplier = gravityMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.gravityMultiplier = 1f;
        Destroy(gameObject);
    }
}
