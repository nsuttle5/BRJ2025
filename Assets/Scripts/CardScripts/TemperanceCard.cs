using UnityEngine;
using System.Collections;

public class TemperanceCard : Card
{
    [SerializeField] private float waitDuration = 1f;

    public override void UseCard()
    {
        PlayerManager.Instance.currentHealth -= 2;
        StartCoroutine(HealOverTime());
    }

    private IEnumerator HealOverTime()
    {
        yield return new WaitForSeconds(waitDuration);
        PlayerManager.Instance.currentHealth = PlayerManager.Instance.maxHealth;
        Destroy(gameObject);
    }
}
