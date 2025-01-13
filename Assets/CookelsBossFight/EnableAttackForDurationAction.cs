using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnableAttackForDuration", story: "[Controller] enables [Attack] for [Duration] seconds", category: "Action", id: "b781ec746c9cc517bee570e208ba6090")]
public partial class EnableAttackForDurationAction : Action
{
    [SerializeReference] public BlackboardVariable<CookelsMechanicsController> Controller;
    [SerializeReference] public BlackboardVariable<CookelsAttackEnum> Attack;
    [SerializeReference] public BlackboardVariable<float> Duration;

    private float startTime; 
    
    protected override Status OnStart()
    {
        Controller.Value.EnableAttack(Attack.Value);
        startTime = Time.time; // Store the start time when the action begins
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Check if enough time has elapsed
        if (Time.time - startTime >= Duration.Value)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd() {
        // When we have finished loading we disable the attack
        Controller.Value.DisableAttack(Attack.Value);
    }
}

