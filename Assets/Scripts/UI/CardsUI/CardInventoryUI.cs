using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInventoryUI : MonoBehaviour {
    [SerializeField] private GameObject inventoryCardPrefab;
    [SerializeField] private Transform unlockedCardsSlot;

    private List<CardSO> unlockedCard;

    private void Start() {
        unlockedCard = CardInventoryManager.Instance.GetUnlockedCardsList();
        
        if (unlockedCardsSlot.childCount != unlockedCard.Count) {
            RearrangeCardsInInventory();
        }
    }
    
    private void RearrangeCardsInInventory() {
        foreach (Transform child in unlockedCardsSlot) {
            Destroy(child.gameObject);
        }
        foreach (CardSO card in unlockedCard) {
            GameObject inventoryCard = Instantiate(inventoryCardPrefab, unlockedCardsSlot, false);
            inventoryCard.GetComponent<InventoryCard>().cardSO = card;
            inventoryCard.GetComponent<Image>().sprite = card.cardImage;

            if (CardInventoryManager.Instance.GetEquippedCards().Contains(card)) {
                inventoryCard.GetComponent<InventoryCard>().isSelected = true;
            }
        }
    }
}
