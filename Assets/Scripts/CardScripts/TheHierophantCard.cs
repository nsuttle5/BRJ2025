using System.Collections;
using UnityEngine;

public class TheHierophantCard : Card
{
    [SerializeField] private float buffDuration;

    public override void UseCard()
    {
        PlayerManager.Instance.GetPlayerMovement().CanDoubleJump(true);
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        PlayerManager.Instance.GetPlayerMovement().CanDoubleJump(false);
    }


}
