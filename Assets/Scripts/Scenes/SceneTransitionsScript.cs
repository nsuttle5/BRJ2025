using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsScript : MonoBehaviour
{
        

    public string sceneToLoad;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            LoadScene();
        }
    }


    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
