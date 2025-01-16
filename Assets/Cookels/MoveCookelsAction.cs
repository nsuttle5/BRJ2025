using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveCookels", story: "[Boss] moves to [Target] with speed: [MoveSpeed]", category: "Action", id: "f798ebccb76103313c7023e754e6d307")]
public partial class MoveCookelsAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Boss;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;
    
    // Distance threshold to consider position reached
    private float arrivalThreshold = 0.1f;
    
    protected override Status OnStart()
    {
        // Disable the boss RB
        Boss.Value.GetComponent<Rigidbody>().isKinematic = true;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Get current position and target position
        Vector3 currentPosition = Boss.Value.transform.position;
        Vector3 targetPosition = Target.Value.position;
        
        // Calculate distance to target
        float distanceToTarget = Vector3.Distance(currentPosition, targetPosition);
        
        // Check if we've reached the target
        if (distanceToTarget <= arrivalThreshold)
        {
            Boss.Value.GetComponent<Rigidbody>().isKinematic = false;
            return Status.Success;
        }
        
        // Calculate movement
        Vector3 direction = (targetPosition - currentPosition).normalized;
        Vector3 movement = direction * MoveSpeed * Time.deltaTime;
        
        // Move towards target
        Boss.Value.transform.position += movement;
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

