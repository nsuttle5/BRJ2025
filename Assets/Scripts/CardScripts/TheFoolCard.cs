using System.Collections;
using UnityEngine;

public class TheFoolCard : Card
{
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        //Reduce move speed for certain duration
        PlayerManager.Instance.SetMoveSpeed(PlayerManager.Instance.GetMoveSpeed() * moveSpeedMultiplier);
        StartCoroutine(BuffDuration());

        //Increase health by one
        PlayerManager.Instance.Heal(1);
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        PlayerManager.Instance.SetMoveSpeed(PlayerManager.Instance.GetMoveSpeed() / moveSpeedMultiplier);
        Destroy(gameObject);
    }
}
