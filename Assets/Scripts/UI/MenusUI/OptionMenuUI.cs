using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuUI : MonoBehaviour
{
    private const string FULLSCREEN = "FULLSCREEN";
    private const string WINDOWED = "WINDOWED";
    private const string START_SCENE = "StartScreen";

    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button screenTypeButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI screenTypeText;
    private bool isFullscreen;
    private SceneLoader sceneLoader;

    private void Start() {

        isFullscreen = Screen.fullScreen;
        sceneLoader = SceneLoader.Instance;

        soundEffectSlider.onValueChanged.AddListener(value => {
            //Add Logic here after we got some sound
            Debug.Log("Sound effect volume : " + value);
        });
        musicSlider.onValueChanged.AddListener(value => {
            //Add Logic here after we got some music
            Debug.Log("Music volume : " + value);
        });
        screenTypeButton.onClick.AddListener(() => {
            ChangeScreenType();
        });
        closeButton.onClick.AddListener(() => {
            //Switch scene or close option window
            sceneLoader.LoadScene(SceneLoader.Scene.StartScene);
        });
    }

    private void ChangeScreenType()
    {
        if (isFullscreen)
        {
            Screen.fullScreen = false;
            isFullscreen = false;
            screenTypeText.text = WINDOWED;
        }
        else
        {
            Screen.fullScreen = true;
            isFullscreen = true;
            screenTypeText.text = FULLSCREEN;
        }
    }
}
