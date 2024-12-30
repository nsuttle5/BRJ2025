using UnityEngine;

public class HealCard : Card
{
    public override void UseCard()
    {
        //Apply Dash Logic
        Debug.Log("Player healed");
    }
}
