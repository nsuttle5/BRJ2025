using System.Collections;
using UnityEngine;

public class TheFoolCard : Card
{
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        //Reduce move speed for certain duration
        StatsHandler.Instance.moveSpeedMultiplier = moveSpeedMultiplier;
        StartCoroutine(BuffDuration());

        //Increase health by one
        PlayerManager.Instance.Heal(1);
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        StatsHandler.Instance.moveSpeedMultiplier = 1f;
        Destroy(gameObject);
    }
}
