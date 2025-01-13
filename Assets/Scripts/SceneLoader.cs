using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader Instance;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingProgressBar;

    private void Awake() {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
        loadingScreen.SetActive(false);
        loadingProgressBar.fillAmount = 0;
    }

    //Add the Name of all scenes in here
    public enum Scene {
        StartScene,
        PlayerHutScene,
        OverworldScene,
        OptionScene,
    }

    public async void LoadScene(Scene sceneName) {
        loadingScreen.SetActive(true);
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName.ToString());
        scene.allowSceneActivation = false;

        do {
            await Task.Delay(1000);
            loadingProgressBar.fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;

        await Task.Delay(1000);
        loadingScreen.SetActive(false);
        loadingProgressBar.fillAmount = 0;
    }
    public void QuitGame() => Application.Quit();

}
