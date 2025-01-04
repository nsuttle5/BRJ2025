using UnityEngine;
using System.Collections;

public class TemperanceCard : Card
{
    public float waitDuration = 1f;

    public override void UseCard()
    {
        PlayerManager.Instance.Heal(-2);
        StartCoroutine(HealOverTime());
    }

    private IEnumerator HealOverTime()
    {
        yield return new WaitForSeconds(waitDuration);
        PlayerManager.Instance.Heal(10);
        Destroy(gameObject);
    }
}
