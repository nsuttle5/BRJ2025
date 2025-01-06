using UnityEngine;
using System.Collections;

public class TheJusticeCard : Card
{
    [SerializeField] private float invincibleDuration = 5f;
    public override void UseCard()
    {
        PlayerManager.Instance.SetInvincibility(true);
        StartCoroutine(BuffDuration());
    }

    private IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(invincibleDuration);
        PlayerManager.Instance.SetInvincibility(false);
        Destroy(gameObject);
    }
}
