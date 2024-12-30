using System.Collections.Generic;
using UnityEngine;

public class CardsManagerUI : MonoBehaviour
{
    [SerializeField] private Transform[] cardSlots; 
    [SerializeField] private bool[] availableCardSlot; //Bool to check if its free to use
    [SerializeField] private List<CardSO> cardList;
    [SerializeField] private List<CardSO> tempCards; //Temporary storage for cards

    private int selectedCardIndex = 0;
    private int currentNumberOfCards = 0;

    private Vector3 selectedSize = new Vector3(1.2f, 1.2f, 1.2f);

    private void Awake()
    {
        cardList = new List<CardSO>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            //Select next card
            if (selectedCardIndex < cardList.Count - 1) selectedCardIndex++;
            else selectedCardIndex = 0;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            //Select Previous card
            if (selectedCardIndex > 0) selectedCardIndex--;
            else selectedCardIndex = cardList.Count - 1;
        }

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (i == selectedCardIndex) cardSlots[i].localScale = selectedSize;
            else cardSlots[i].localScale = Vector3.one;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseCard();
        }
    }

    public void AddCard(CardSO newCard)
    {
        if (CanAddMoreCards())
        {
            for (int i = 0; i < cardSlots.Length; i++)
            {
                if (availableCardSlot[i])
                {
                    cardList.Add(newCard);
                    Instantiate(newCard.cardPrefab, cardSlots[i], false);
                    availableCardSlot[i] = false;
                    currentNumberOfCards++;
                    return;
                }
            }
        }
    }

    public void UseCard()
    {
        if (cardList.Count > 0)
        {
            cardList[selectedCardIndex].cardPrefab.UseCard();
            cardList.RemoveAt(selectedCardIndex);
        }
        RearrangeCards();
    }

    public bool CanAddMoreCards()
    {
        if (currentNumberOfCards < cardSlots.Length) return true;
        else return false;
    }

    private void RearrangeCards()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            availableCardSlot[i] = true;
            if (cardSlots[i].childCount > 0)
            {
                Destroy(cardSlots[i].GetChild(0).gameObject);
            }
        }
        foreach (CardSO card in cardList)
        {
            tempCards.Add(card);
        }
        cardList.Clear();
        foreach (CardSO card in tempCards)
        {
            if (CanAddMoreCards()) AddCard(card);
        }
        tempCards.Clear();
        currentNumberOfCards = 0;
        selectedCardIndex = 0;
    }
}
