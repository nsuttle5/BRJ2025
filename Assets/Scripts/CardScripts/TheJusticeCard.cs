using UnityEngine;
using System.Collections;

public class TheJusticeCard : Card
{
    public float invincibleDuration = 5f;
    public override void UseCard()
    {
        PlayerManager.Instance.isInvincible = true;
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(invincibleDuration);
        PlayerManager.Instance.isInvincible = false;
        Destroy(gameObject);
    }
}
