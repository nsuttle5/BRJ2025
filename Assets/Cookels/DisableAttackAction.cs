using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DisableAttackAction", story: "[Controller] disable [Attack]", category: "Action", id: "d1d03f0cf5ed5e9f267bb2b1904413c5")]
public partial class DisableAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<CookelsMechanicsController> Controller;
    [SerializeReference] public BlackboardVariable<CookelsAttackEnum> Attack;

    protected override Status OnStart() {
        Controller.Value.DisableAttack(Attack.Value);
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

