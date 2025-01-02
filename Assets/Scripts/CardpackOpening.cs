using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardpackOpening : MonoBehaviour
{
    [Header("Cardpack Settings")]
    public int cardpackNum = 3;
    public GameObject[] cardpackSlots;
    public Transform[] cardPositions;

    public GameObject cardPackSlotPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //cardpackSlots = new GameObject[cardpackRows, cardpackColumns];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenCardpack()
    {
        Destroy(cardPackSlotPrefab);
        StartCoroutine(SpawnCards());
    }

    private IEnumerator SpawnCards()
    {
        for (int i = 0; i < cardpackSlots.Length; i++)
        {
            Instantiate(cardpackSlots[i], cardPositions[i].position, cardPositions[i].rotation);
            yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
        }
    }
}
