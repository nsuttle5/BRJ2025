using System.Collections.Generic;
using UnityEngine;

public class CardInventoryManager : MonoBehaviour
{
    public static CardInventoryManager Instance;

    [SerializeField] private List<CardSO> cardsList;
    [SerializeField] private int maxCardToUse = 12;

    private List<CardSO> unlockedCardsList;
    private List<CardSO> equippedCards;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject);

        unlockedCardsList = new List<CardSO>();
        equippedCards = new List<CardSO>();

        unlockedCardsList = cardsList;
    }


    //Use this function to unlock new types of card.
    public void UnlockCard(CardSO card)
    {
        unlockedCardsList.Add(card);
    }
    public List<CardSO> GetUnlockedCardsList() => unlockedCardsList;
    public List<CardSO> GetEquippedCards() => equippedCards;
    public bool CanEquipMoreCards() => equippedCards.Count < maxCardToUse;
}
