using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardsManagerUI : MonoBehaviour
{
    [SerializeField] private Transform[] cardSlots; 
    [SerializeField] private List<CardSO> cardList;
    [SerializeField] private TextMeshProUGUI currentCardNameText;

    private InputManager inputManager;

    private bool canScroll;
    private float scrollBufferTime = 0.2f;
    private float scrollBufferTimer;

    private bool[] availableCardSlot; //Bool to check if its free to use
    private int selectedCardIndex = 0;
    private int currentNumberOfCards = 0;

    private Vector3 selectedSize = new Vector3(1.2f, 1.2f, 1.2f);

    private void Awake() {
        availableCardSlot = new bool[cardSlots.Length];
        cardList = new List<CardSO>();
        for (int i = 0; i < availableCardSlot.Length; i++) availableCardSlot[i] = true;
        scrollBufferTimer = scrollBufferTime;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.OnUseCardPressed += InputManager_OnUseCardPressed;
    }

    private void InputManager_OnUseCardPressed(object sender, System.EventArgs e) {
        UseCard();
    }

    private void Update() {
        if (inputManager.GetScrollCardAxis() > 0 && canScroll) {
            //Select next card
            if (selectedCardIndex < cardList.Count - 1) selectedCardIndex++;
            else selectedCardIndex = 0;
            canScroll = false;
        }
        else if(inputManager.GetScrollCardAxis() < 0 && canScroll) {
            //Select Previous card
            if (selectedCardIndex > 0) selectedCardIndex--;
            else selectedCardIndex = cardList.Count - 1;
            canScroll = false;
        }
        else if (!canScroll) {
            scrollBufferTimer -= Time.deltaTime;
        }

        if (scrollBufferTimer  <= 0 && !canScroll) {
            canScroll = true;
            scrollBufferTimer = scrollBufferTime;
        }

        for (int i = 0; i < cardSlots.Length; i++) {
            if (i == selectedCardIndex) cardSlots[i].localScale = selectedSize;
            else cardSlots[i].localScale = Vector3.one;

            if (cardSlots[i].childCount > 0) {
                float swapSpeed = 10f;
                Transform card = cardSlots[i].GetChild(0);
                if (card.localPosition != Vector3.zero || card.localRotation != Quaternion.identity) {
                    card.localPosition = Vector3.Lerp(card.localPosition, Vector3.zero, Time.deltaTime * swapSpeed);
                    card.localRotation = Quaternion.Lerp(card.localRotation, Quaternion.identity, Time.deltaTime * swapSpeed);
                }
            }
        }

        if (cardList.Count > 0) {
            currentCardNameText.text = cardList[selectedCardIndex].cardName;
        }
        else currentCardNameText.text = "";
    }

    public void AddCard(CardSO newCard) {
        if (CanAddMoreCards()) {
            for (int i = 0; i < cardSlots.Length; i++) {
                if (availableCardSlot[i]) {
                    cardList.Add(newCard);
                    Instantiate(newCard.cardPrefab, cardSlots[i], false);
                    availableCardSlot[i] = false;
                    currentNumberOfCards++;
                    return;
                }
            }
        }
    }
    public void UseCard() {
        if (cardList.Count > 0) {
            cardList.RemoveAt(selectedCardIndex);
            availableCardSlot[selectedCardIndex] = true;
            Transform card = cardSlots[selectedCardIndex].GetChild(0);
            card.GetComponent<Card>().UseCard();
            card.SetParent(null);
            currentNumberOfCards--;
            
            if (selectedCardIndex >= currentNumberOfCards) selectedCardIndex = 0;
        }
        RearrangeCards();
    }

    public bool CanAddMoreCards() {
        if (currentNumberOfCards < cardSlots.Length) return true;
        else return false;
    }

    private void RearrangeCards() {
        for(int i = 0; i < cardSlots.Length - 1; i++) {
            if (availableCardSlot[i] && !availableCardSlot[i + 1] && cardSlots[i + 1].childCount > 0) {
                        Transform card = cardSlots[i + 1].GetChild(0);
                        card.SetParent(cardSlots[i]);
                        card.localScale = Vector3.one;
                        availableCardSlot[i] = false;
                        availableCardSlot[i + 1] = true;
            }
        }
    }
}
