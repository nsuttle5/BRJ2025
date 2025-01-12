using System;
using UnityEngine;

public class CookelsController : MonoBehaviour {
    public enum CookelsCurrentAttack {
        None,
        RidingOnStage,
        BlowingUpBalloons,
        RubberBall
    }
    
    public CookelsCurrentAttack currentAttack = CookelsCurrentAttack.None;

    [Header("Cookels Attack references")] 
    public CookelsBycicleAttack BycicleAttack;
    public CookelsBalloonAttack BalloonAttack;

    public void ActivateRideOnStage() {
        currentAttack = CookelsCurrentAttack.RidingOnStage;
    }
}
