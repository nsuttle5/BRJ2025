using UnityEngine;
using System.Collections;

public class TemperanceCard : Card
{
    [SerializeField] private float waitDuration = 1f;

    public override void UseCard()
    {
        PlayerManager.Instance.SetCurrentHealth(2);
        StartCoroutine(HealOverTime());
    }

    private IEnumerator HealOverTime()
    {
        yield return new WaitForSeconds(waitDuration);
        PlayerManager.Instance.SetCurrentHealth(PlayerManager.Instance.GetMaxHealth());
        Destroy(gameObject);
    }
}
