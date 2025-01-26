using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CookelsBycicleAttack", story: "Bicycle Attack", category: "Action", id: "e26460d06cfa6cabce8d8021fd5fcaf1")]
public partial class CookelsBycicleAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Laps;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;
    [SerializeReference] public BlackboardVariable<GameObject> CookelsGameObject;
    
    [SerializeReference] public BlackboardVariable<Transform> StageCenterTransform;

    [SerializeReference] public BlackboardVariable<float> StageRadius;
    
    [SerializeReference] public BlackboardVariable<bool> Clockwise;
    [SerializeReference] public BlackboardVariable<bool> SpriteFacesMovementDirection;

    private Animator cookelsAnimator;
    private AnimationStateHandler animationStateHandler;

    // Animation constants
    private const string ATTACK_ANTICIPATION_STATE_NAME = "BycicleAttack_Anticipation"; // This needs to match the animator state name
    private const string ATTACK_STATE_NAME = "BycicleAttack";
    private const string ATTACK_END_STATE_NAME = "BycicleAttack_End"; // This needs to match the animator state name
    private const string IDLE_STATE_NAME = "IdleCookels";

    // State tracking
    private float currentAngle;
    private float elapsedTime;
    private float totalRotation;
    private AttackPhase currentPhase;

    private enum AttackPhase {
        Anticipation,
        Rotating,
        Ending,
        Complete
    }
    
    protected override Status OnStart() {

        if (!ValidateReferences())
            return Status.Failure;

        cookelsAnimator = CookelsGameObject.Value.GetComponent<Animator>();
        animationStateHandler = CookelsGameObject.Value.GetComponent<AnimationStateHandler>();

        // Initialize state
        currentPhase = AttackPhase.Anticipation;
        elapsedTime = 0f;
        totalRotation = 0f;

        // Start anticipation animation
        cookelsAnimator.Play(ATTACK_ANTICIPATION_STATE_NAME);

        return Status.Running;
    }

    protected override Status OnUpdate() {
        elapsedTime += Time.deltaTime;

        switch (currentPhase) {
            case AttackPhase.Anticipation:
                if (animationStateHandler.hasCurrentAnimationEnded) {
                    currentPhase = AttackPhase.Rotating;
                    cookelsAnimator.Play(ATTACK_STATE_NAME);
                    animationStateHandler.OnStartNewAnimation();
                    elapsedTime = 0f;
                }
                break;

            case AttackPhase.Rotating:
                HandleMovementAndRotation();
                CheckRotationComplete();
                break;

            case AttackPhase.Ending:
                if (animationStateHandler.hasCurrentAnimationEnded) {
                    currentPhase = AttackPhase.Complete;
                    cookelsAnimator.Play(IDLE_STATE_NAME);
                    animationStateHandler.OnStartNewAnimation();
                    return Status.Success;
                }
                break;

            case AttackPhase.Complete:
                return Status.Success;
        }

        return Status.Running;
    }
    
    private void HandleMovementAndRotation() {
        // Update the angle (clockwise or counter-clockwise)
        float direction = Clockwise.Value ? -1f : 1f;
        float angleChange = direction * MoveSpeed.Value * Time.deltaTime;
        currentAngle += angleChange;
        totalRotation += Mathf.Abs(angleChange);

        // Calculate new position on X and Z axes
        float newX = StageCenterTransform.Value.position.x + StageRadius.Value * Mathf.Cos(currentAngle);
        float newZ = StageCenterTransform.Value.position.z + StageRadius.Value * Mathf.Sin(currentAngle);

        // Update position
        Transform cookelsTransform = CookelsGameObject.Value.transform;
        cookelsTransform.position = new Vector3(newX, cookelsTransform.position.y, newZ);

        // Rotate sprite to face movement direction
        if (SpriteFacesMovementDirection) {
            float angle = Mathf.Atan2(newZ - StageCenterTransform.Value.position.z, 
                newX - StageCenterTransform.Value.position.x) * Mathf.Rad2Deg;
            cookelsTransform.rotation = Quaternion.Euler(0, angle - 90, 0);
        }
    }

    private void CheckRotationComplete() {
        float rotationForOneLap = 2f * Mathf.PI;
        if (totalRotation >= rotationForOneLap * Laps.Value) {
            currentPhase = AttackPhase.Ending;
            elapsedTime = 0f;
            cookelsAnimator.Play(ATTACK_END_STATE_NAME);
        }
    }

    protected override void OnEnd() {
        // Ensure we're in idle state if interrupted
        if (currentPhase != AttackPhase.Complete) {
            cookelsAnimator.Play(IDLE_STATE_NAME);
        }
    }
    
    private bool ValidateReferences() {
        if (CookelsGameObject.Value == null ||
            StageCenterTransform.Value == null || 
            Laps.Value <= 0)
        {
            Debug.LogError("CookelsBycicleAttackAction: Missing or invalid references");
            return false;
        }
        return true;
    }
}

