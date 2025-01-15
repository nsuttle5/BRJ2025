using UnityEngine;

public class SignInteraction : MonoBehaviour {
    
    private const string IS_HOVERING = "IsHovering";

    public GameObject[] lights; // Add light objects in Inspector
    public Material lightOnMaterial;
    public Material lightOffMaterial;

    public StartSceneCameraManager startSceneCameraManager;

    public enum StartMenuButtonActions {
        StartGame,
        OptionScreen,
        QuitGame
    }
    [SerializeField] private StartMenuButtonActions action;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Turn off lights by default
        SetLights(false);
    }
    

    void OnMouseEnter()
    {
        animator.SetTrigger(IS_HOVERING);
        SetLights(true);
    }

    void OnMouseExit()
    {
        SetLights(false); // Turn lights off
    }

    void OnMouseDown() {
        switch (action) {
            case StartMenuButtonActions.StartGame:
                startSceneCameraManager.Play();
                break;
            case StartMenuButtonActions.OptionScreen:
                SceneLoader.Instance.LoadScene(SceneLoader.Scene.OptionScene);
                break;
            case StartMenuButtonActions.QuitGame:
                SceneLoader.Instance.QuitGame();
                break;
        }
    }

    private void SetLights(bool state)
    {
        foreach (GameObject light in lights)
        {
            var renderer = light.GetComponent<Renderer>();
            renderer.material = state ? lightOnMaterial : lightOffMaterial;
        }
    }
}
