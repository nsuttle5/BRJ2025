using UnityEngine;

public class AnimationStateHandler : MonoBehaviour {

    public bool hasCurrentAnimationEnded = false;
    public void OnAnimationComplete() => hasCurrentAnimationEnded = true;
    public void OnStartNewAnimation() => hasCurrentAnimationEnded = false;
}
