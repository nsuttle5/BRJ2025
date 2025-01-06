using UnityEngine;
using System.Collections;

public class DeathCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        PlayerManager.Instance.SetCurrentHealth(2);
        statsHandler.damageMultiplier = damageMultiplier;
        statsHandler.moveSpeedMultiplier = moveSpeedMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.damageMultiplier = 1f;
        statsHandler.moveSpeedMultiplier = 1f;
        Destroy(gameObject);
    }
}
