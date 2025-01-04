using UnityEngine;

public class TheHighPriestessCard : Card
{
    public override void UseCard()
    {
        PlayerManager.Instance.IncreaseMaxHealth();
        Destroy(gameObject);
    }
}
