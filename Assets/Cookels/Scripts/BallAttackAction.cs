using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BallAttack", story: "Ball Attack", category: "Action", id: "cc096536736faa13d95b1024c32809f9")]
public partial class BallAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> CookelsGameObject;

    [SerializeReference] public BlackboardVariable<GameObject> BallPrefab;
    [SerializeReference] public BlackboardVariable<Transform> BallThrowPoint;

    private const string BALL_PULLOUT_ANTICIPATION_ANIMATION = "";
    private const string BALL_THROW_ANIMATION = "";
    private const string IDLE_STATE_ANIMATION = "";

    private Animator cookelsAnimator;
    private AnimationStateHandler animationStateHandler;
    private AttackPhase currentAttackPhase;
    private enum AttackPhase {
        BallPullOut,
        BallThrow,
        Complete,
    }


    protected override Status OnStart()
    {
        if (!ValidateReferences())
            return Status.Failure;

        cookelsAnimator = CookelsGameObject.Value.GetComponent<Animator>();
        animationStateHandler = CookelsGameObject.Value.GetComponent<AnimationStateHandler>();

        currentAttackPhase = AttackPhase.BallPullOut;
        cookelsAnimator.Play(BALL_PULLOUT_ANTICIPATION_ANIMATION);
        animationStateHandler.OnStartNewAnimation();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        switch (currentAttackPhase) {
            case AttackPhase.BallPullOut:
                if (animationStateHandler.hasCurrentAnimationEnded) {
                    currentAttackPhase = AttackPhase.BallThrow;
                    cookelsAnimator.Play(BALL_THROW_ANIMATION);
                    animationStateHandler.OnStartNewAnimation();
                }
                break;
            case AttackPhase.BallThrow:
                if (animationStateHandler.hasCurrentAnimationEnded) {
                    //Instantiate the ball at ball throw point
                    GameObject.Instantiate(BallPrefab.Value, BallThrowPoint.Value.position, Quaternion.identity);
                    //Back to idle animation
                    currentAttackPhase = AttackPhase.Complete;
                    cookelsAnimator.Play(IDLE_STATE_ANIMATION);
                }
                return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    private bool ValidateReferences() {
        if (CookelsGameObject.Value == null ||
            BallPrefab.Value == null ||
            BallThrowPoint.Value == null) {
            Debug.LogError("CookelsBallAttackAction: Missing or invalid references");
            return false;
        }
        return true;
    }
}

