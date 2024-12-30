using UnityEngine;

public class AttackCard : Card
{
    public override void UseCard()
    {
        //Apply Attack Logic
        Debug.Log("Player Attacked");
    }
}
