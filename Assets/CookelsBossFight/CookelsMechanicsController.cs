using System;
using UnityEngine;

public class CookelsMechanicsController : MonoBehaviour {
    
    private CookelsBycicleAttack bycicleAttack;
    private CookelsBalloonAttack balloonAttack;
    private CookelsBouncyBallAttack bouncyBallAttack;

    private void Start() {
        bycicleAttack = GetComponent<CookelsBycicleAttack>();
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
            case CookelsAttackEnum.BycicleAttack:
                bycicleAttack.Enable();
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
            case CookelsAttackEnum.BycicleAttack:
                bycicleAttack.Disable();
                break;
            case CookelsAttackEnum.BallAttack:
                bouncyBallAttack.Disable();
                break;
        }
    }

}
