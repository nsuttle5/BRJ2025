using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader Instance;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingProgressBar;
    private float fillAmount;

    private void Awake() {
        fillAmount = 0;
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
        loadingScreen.SetActive(false);
        loadingProgressBar.fillAmount = fillAmount;
    }

    private void Update() {
        loadingProgressBar.fillAmount = Mathf.MoveTowards(loadingProgressBar.fillAmount, fillAmount, Time.deltaTime * 4);
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
            fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;

        await Task.Delay(1000);
        loadingScreen.SetActive(false);
        loadingProgressBar.fillAmount = 0;
        fillAmount = 0;
    }
    public void QuitGame() => Application.Quit();

}
