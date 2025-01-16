using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Timer", story: "[Variable] timer decrease", category: "Action", id: "372a69d1f751ad4ada7b41d96a53f5f9")]
public partial class Timercs : Action
{
    [SerializeReference] public BlackboardVariable<float> Variable;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Variable.Value -= Time.deltaTime;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

