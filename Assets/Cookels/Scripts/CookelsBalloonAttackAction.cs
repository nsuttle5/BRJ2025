using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CookelsBalloonAttack", story: "Balloon Attack", category: "Action", id: "59bbe5fa8128d865346e5c1da46affe8")]
public partial class CookelsBalloonAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> CookelsGameObject;

    [SerializeReference] public BlackboardVariable<List<GameObject>> BalloonPrefabList;

    private Animator cookelsAnimator;
    private AnimationStateHandler animationStateHandler;

    private const string BALLOON_PULL_OUT_ANIMATION = "";
    private const string BALLOON_INFLATE_ANIMATION = "";
    private const string BALLOON_RELEASE_ANIMATION = "";
    private const string WAIT_ANIMATION = "";
    private const string END_ANIMATION = "";

    private AttackPhase currentAttackPhase;
    private enum AttackPhase {
        BalloonPullOut,
        BalloonInflate,
        BalloonRelease,
        Wait,
        Complete
    }

    protected override Status OnStart()
    {
        if (!ValidateReferences())
            return Status.Failure;

        cookelsAnimator = CookelsGameObject.Value.GetComponent<Animator>();
        animationStateHandler = CookelsGameObject.Value.GetComponent<AnimationStateHandler>();

        currentAttackPhase = AttackPhase.BalloonPullOut;

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
                break;
            case AttackPhase.BalloonRelease:
                break;
            case AttackPhase.Wait:
                break;
            case AttackPhase.Complete:
                return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    private bool ValidateReferences() {
        if (CookelsGameObject.Value == null) {
            Debug.LogError("CookelsBycicleAttackAction: Missing or invalid references");
            return false;
        }
        return true;
    }
}

