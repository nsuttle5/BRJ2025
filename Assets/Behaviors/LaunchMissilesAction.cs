using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LaunchMissiles", story: "[missileLauncher] launches [missileCount] missiles", category: "Action", id: "c260310f7c37d5f49aaa0722a70c19b1")]
public partial class LaunchMissilesAction : Action
{
    [SerializeReference] public BlackboardVariable<MissileLauncher> MissileLauncher;
    [SerializeReference] public BlackboardVariable<int> MissileCount;

    protected override Status OnStart() {
        MissileLauncher.Value.LaunchMissiles(MissileCount.Value);
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

