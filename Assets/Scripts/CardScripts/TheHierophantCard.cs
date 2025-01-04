using UnityEngine;

public class TheHierophantCard : Card
{

    public override void UseCard()
    {
        PlayerController.Instance.EnableDoubleJump();
        Destroy(gameObject);
    }
}
