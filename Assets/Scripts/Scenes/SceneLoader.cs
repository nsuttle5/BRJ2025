using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader Instance;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingImage;

    private Animator loadingScreenAnimator;
    private Scene targetScene;

    private void Awake() {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
        loadingScreen.SetActive(true);
        loadingImage.fillAmount = 0;
        loadingScreenAnimator = gameObject.GetComponent<Animator>();
    }
    

    //Add the Name of all scenes in here
    public enum Scene {
        StartScene,
        PlayerHutScene,
        OverworldScene,
        OptionScene,
    }

    public void LoadScene(Scene sceneName) {
        targetScene = sceneName;
        loadingScreen.SetActive(true);
        loadingScreenAnimator.Play("LoadIn");
    }
    public AsyncOperation LoadCallback() {
        return SceneManager.LoadSceneAsync(targetScene.ToString());
    }
    public void QuitGame() => Application.Quit();
    public void LoadFromAnimation() => SceneManager.LoadScene("LoadingScene");
    public void LoadOut() => loadingScreenAnimator.Play("LoadOut");
    public void SetLoadingScreenActive(bool condition) => loadingScreen.SetActive(condition);
    public void SetLoadingScreenToInactive() => loadingScreen.SetActive(false); //For animator 
}
