using UnityEngine;

public class LoadingCallback : MonoBehaviour
{
    private SceneLoader loader;

    AsyncOperation sceneLoad;

    private void Start() {
        loader = SceneLoader.Instance;
        sceneLoad = loader.LoadCallback();
        loader.SetLoadingScreenActive(false);
        sceneLoad.allowSceneActivation = false;
    }

    private void Update() {
        if (loader == null) return;

        if (sceneLoad.progress >= 0.9f) {
            sceneLoad.allowSceneActivation = true;
            loader.SetLoadingScreenActive(true);
            loader.LoadOut();
            loader = null;
        }
    }

}
