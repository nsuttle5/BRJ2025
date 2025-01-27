using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CookelsBalloonAttack", story: "Balloon Attack", category: "Action", id: "59bbe5fa8128d865346e5c1da46affe8")]
public partial class CookelsBalloonAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> CookelsGameObject;

    [SerializeReference] public BlackboardVariable<List<GameObject>> BalloonPrefabList;
    [SerializeReference] public BlackboardVariable<Transform> BalloonSpawnPoint;

    private Animator cookelsAnimator;
    private AnimationStateHandler animationStateHandler;

    private const string BALLOON_PULL_OUT_ANIMATION = "";
    private const string BALLOON_INFLATE_ANIMATION = "";

    private AttackPhase currentAttackPhase;
    private int numberOfBalloonInflated;

    private enum AttackPhase {
        BalloonPullOut,
        BalloonInflate,
        Complete
    }

    protected override Status OnStart()
    {
        if (!ValidateReferences())
            return Status.Failure;

        cookelsAnimator = CookelsGameObject.Value.GetComponent<Animator>();
        animationStateHandler = CookelsGameObject.Value.GetComponent<AnimationStateHandler>();

        currentAttackPhase = AttackPhase.BalloonPullOut;
        numberOfBalloonInflated = 0;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        switch (currentAttackPhase) {
            case AttackPhase.BalloonPullOut:
                cookelsAnimator.Play(BALLOON_PULL_OUT_ANIMATION);
                animationStateHandler.OnStartNewAnimation();
                if (animationStateHandler.hasCurrentAnimationEnded) {
                    currentAttackPhase = AttackPhase.BalloonInflate;
                }
                break;
            case AttackPhase.BalloonInflate:
                cookelsAnimator.Play(BALLOON_INFLATE_ANIMATION);
                animationStateHandler.OnStartNewAnimation();
                if (animationStateHandler.hasCurrentAnimationEnded) {
                    //Spawn Balloon and Place it to the desired Place
                    GameObject.Instantiate(BalloonPrefabList.Value[numberOfBalloonInflated], BalloonSpawnPoint.Value, false);
                    numberOfBalloonInflated++;
                    //If all Balloons inflated => go to next state
                    //Else => go back to previous state
                    if (numberOfBalloonInflated < BalloonPrefabList.Value.Count) {
                        currentAttackPhase = AttackPhase.BalloonPullOut;
                    }
                    else {
                        currentAttackPhase = AttackPhase.Complete;
                    }
                }
                break;
            case AttackPhase.Complete:
                return Status.Success;
        }

        return Status.Running;
    }

    private bool ValidateReferences() {
        if (CookelsGameObject.Value == null) {
            Debug.LogError("CookelsBalloonAttackAction: Missing or invalid references");
            return false;
        }
        return true;
    }
}

