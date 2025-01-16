using UnityEngine;

public class CookelsMechanicsController : MonoBehaviour {
    
    private CookelsBalloonAttack balloonAttack;
    private CookelsBouncyBallAttack bouncyBallAttack;

    private void Start() {
        balloonAttack = GetComponent<CookelsBalloonAttack>();
        bouncyBallAttack = GetComponent<CookelsBouncyBallAttack>();
    }

    public void EnableAttack(CookelsAttackEnum attack) {
        Debug.Log("Enabling attack: ");
        Debug.Log(attack);
        // ToDo: might be better to implement this as an interface and just call Enable()
        switch (attack) {
            case CookelsAttackEnum.BalloonAttack:
                balloonAttack.Enable();
                break;
            case CookelsAttackEnum.BycycleAttack:
                break;
            case CookelsAttackEnum.BallAttack:
                bouncyBallAttack.Enable();
                break;
        }
    }
    
    public void DisableAttack(CookelsAttackEnum attack) {
        // ToDo: might be better to implement this as an interface and just call Disable()
        Debug.Log("Disabling attack: ");
        Debug.Log(attack);
        switch (attack) {
            case CookelsAttackEnum.BalloonAttack:
                balloonAttack.Disable();
                break;
            case CookelsAttackEnum.BycycleAttack:
                break;
            case CookelsAttackEnum.BallAttack:
                bouncyBallAttack.Disable();
                break;
        }
    }

}
