using UnityEngine;
using UnityEngine.UI;

public class TarotTable : MonoBehaviour
{
    public GameObject uiPrompt;
    public GameObject uiBox;
    private bool isPlayerInRange = false;
    private bool isUIBoxOpen = false;

    void Start()
    {
        uiPrompt.SetActive(false);
        uiBox.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isUIBoxOpen = !isUIBoxOpen;
            uiBox.SetActive(isUIBoxOpen);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            uiPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            uiPrompt.SetActive(false);
            uiBox.SetActive(false);
            isUIBoxOpen = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiPrompt.SetActive(true);
        }
    }
}