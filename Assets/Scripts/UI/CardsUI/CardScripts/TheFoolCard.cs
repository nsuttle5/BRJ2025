using System.Collections;
using UnityEngine;

public class TheFoolCard : Card
{
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float buffDuration;

    private float defaultMoveSpeed;

    public override void UseCard()
    {
        //Reduce move speed for certain duration
        defaultMoveSpeed = PlayerManager.Instance.GetMoveSpeed();
        PlayerManager.Instance.SetMoveSpeed(PlayerManager.Instance.GetMoveSpeed() * moveSpeedMultiplier);
        //StartCoroutine(BuffDuration());

        //Increase health by one
        PlayerManager.Instance.Heal(1);
    }

    //private IEnumerator BuffDuration()
    //{
    //    yield return new WaitForSeconds(buffDuration);
    //    PlayerManager.Instance.SetMoveSpeed(defaultMoveSpeed);
    //    Destroy(gameObject);
    //}
}
