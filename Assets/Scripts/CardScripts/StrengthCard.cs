using UnityEngine;
using System.Collections;

public class StrengthCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        PlayerManager.Instance.GetPlayerMovement().CanDoubleJump(true);
        statsHandler.jumpHeightMultiplier = jumpHeightMultiplier;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        PlayerManager.Instance.GetPlayerMovement().CanDoubleJump(true);
        statsHandler.jumpHeightMultiplier = 1f;
        Destroy(gameObject);
    }
}
