using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    [SerializeField] private CardSO[] cards;
    [SerializeField] private CardsManagerUI cardsManagerUI;
    //Temporary Script for adding cards
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && cardsManagerUI.CanAddMoreCards())
        {
            cardsManagerUI.AddCard(cards[Random.Range(0, cards.Length)]);
        }
    }
}