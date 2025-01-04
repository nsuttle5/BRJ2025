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
        healthDec();
        statsHandler.damageMultiplier = damageMultiplier;
        statsHandler.moveSpeedMultiplier = moveSpeedMultiplier;
        StartCoroutine(BuffDuration());
    }

    public void healthDec()
    {
        for (int i = PlayerManager.Instance.currentHealth; i > 2; i--)
        {
            PlayerManager.Instance.Heal(-1);
        }
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        statsHandler.damageMultiplier = 1f;
        statsHandler.moveSpeedMultiplier = 1f;
        Destroy(gameObject);
    }
}
