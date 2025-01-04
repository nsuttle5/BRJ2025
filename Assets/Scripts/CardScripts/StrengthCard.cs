using UnityEngine;
using System.Collections;

public class StrengthCard : Card
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        PlayerController.Instance.EnableDoubleJump();
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
