using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayAnimatorState", story: "[Animator] plays [StateName]", category: "Action", id: "cbce849d791eef576af0694f402e2a82")]
public partial class PlayAnimatorStateAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<string> StateName;

    protected override Status OnStart()
    {
        Animator.Value.Play(StateName.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

