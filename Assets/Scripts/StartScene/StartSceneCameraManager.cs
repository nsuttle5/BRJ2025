using UnityEngine;

public class StartSceneCameraManager : MonoBehaviour
{
    private const string GO_INSIDE_TENT = "GoInsideTent";
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    

    public void Play()
    {
        animator.SetTrigger(GO_INSIDE_TENT);
    }
    public void OpenOverworldScene() => SceneLoader.Instance.LoadScene(SceneLoader.Scene.OverworldScene);
}
