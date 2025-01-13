using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnableAttack", story: "[Controller] enables [attack]", category: "Action", id: "f0edff15c59d823649beabd5cc3ebcd1")]
public partial class EnableAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<CookelsMechanicsController> Controller;
    [SerializeReference] public BlackboardVariable<CookelsAttackEnum> Attack;

    protected override Status OnStart() {
        Controller.Value.EnableAttack(Attack.Value);
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

