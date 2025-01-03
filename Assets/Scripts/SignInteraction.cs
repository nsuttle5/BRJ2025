using UnityEngine;
using System.Collections;

public class SignInteraction : MonoBehaviour
{
    public GameObject[] lights; // Add light objects in Inspector
    public Material lightOnMaterial;
    public Material lightOffMaterial;

    public LevelManager levelManager; // Reference to the LevelManager script
    public string action; // "play", "options", or "quit" (set this per sign in Inspector)
    public StartSceneCameraManager startSceneCameraManager;

    private Animator animator;
    private bool isHovered = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Turn off lights by default
        SetLights(false);
    }

    void Update()
    {
        // Check hover state and toggle animation
        if (isHovered)
        {
            if (!animator.GetBool("IsHovering"))
                animator.SetBool("IsHovering", true);
        }
        else
        {
            if (animator.GetBool("IsHovering"))
                animator.SetBool("IsHovering", false);
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
        SetLights(true); // Turn lights on
    }

    void OnMouseExit()
    {
        isHovered = false;
        SetLights(false); // Turn lights off
    }

    void OnMouseDown()
    {
        if (isHovered)
        {
            // Perform the action based on the sign
            switch (action.ToLower())
            {
                case "play":
                    StartCoroutine(startGame());
                    break;
                case "options":
                    levelManager.LoadOptions();
                    break;
                case "quit":
                    levelManager.QuitGame();
                    break;
                default:
                    Debug.LogWarning("Action not defined for this sign!");
                    break;
            }
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

    IEnumerator startGame()
    {
        if (startSceneCameraManager != null)
        {
            startSceneCameraManager.Play();
            yield return new WaitForSeconds(startSceneCameraManager.animationTime);
        }
        else
        {
            Debug.LogWarning("startSceneCameraManager is not assigned!");
            yield break;
        }
        levelManager.LoadScene();
    }
}
