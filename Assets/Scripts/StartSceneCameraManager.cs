using UnityEngine;

public class StartSceneCameraManager : MonoBehaviour
{

    public LevelManager levelManager; // Reference to the LevelManager script
    public string action; // "play", "options", or "quit" (set this per sign in Inspector)

    private Animator animator;
    private bool hasStarted = false;
    private Animator blackScreenAnimator;

    public float animationTime = 1.5f;

    public GameObject blackScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        blackScreenAnimator = blackScreen.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            animator.SetBool("HasStarted", true);
            blackScreenAnimator.SetBool("FadeOut", true);
        }
    }
}
