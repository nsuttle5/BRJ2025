using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardDeckManager : MonoBehaviour
{
    [Header("Cardpack Settings")]
    [SerializeField] private CardsManagerUI cardsManagerUI;
    [SerializeField] private CardShow[] cardShows;
    [SerializeField] private Image timeToReuseFillBar;
    private List<CardSO> availabeCardsList;

    private bool canOpenDeck = true;
    [SerializeField] private float reuseTime;
    private float reuseTimer = 0;


    private void Awake()
    {
        HideCards();
    }

    private void Start()
    {
        InputManager.Instance.OnCardDeckOpened += InputManager_OnCardDeckOpened;
        availabeCardsList = CardInventoryManager.Instance.GetUnlockedCardsList();
        foreach (CardSO card in availabeCardsList) Debug.Log(card.cardName);

        foreach (CardShow cardShow in cardShows)
        {
            cardShow.OnSelectButtonClicked += CardShow_OnSelectButtonClicked;
        }
    }

    private void InputManager_OnCardDeckOpened(object sender, System.EventArgs e) {
        if (canOpenDeck && cardsManagerUI.CanAddMoreCards()) {
            if (reuseTimer <= 0) {
                for (int i = 0; i < cardShows.Length; i++) {
                    //Select 3 card from the list (Also consider the rarity value)
                    CardSO selectedCard = SelectCard(availabeCardsList);

                    //There might me an issue here which I will fix when we complete the card system 
                    if (selectedCard == null) {
                        Debug.LogError("As expected there is a null refrence in selected card", selectedCard);
                        return;
                    }

                    //From the selected 3 card place each of them on the card slot pack
                    CardShow cardShow = cardShows[i];
                    cardShow.gameObject.SetActive(true);
                    cardShow.cardNameText.text = selectedCard.cardName;
                    cardShow.cardDescriptionText.text = selectedCard.description;
                    cardShow.cardImage.sprite = selectedCard.cardImage;
                    switch (selectedCard.rarity) {
                        case CardSO.Rarity.COMMON:
                            cardShow.rarityText.text = "COMMON";
                            break;
                        case CardSO.Rarity.UNCOMMON:
                            cardShow.rarityText.text = "UNCOMMON";
                            break;
                        case CardSO.Rarity.Rare:
                            cardShow.rarityText.text = "RARE";
                            break;
                    }
                    cardShow.cardSO = selectedCard;

                    canOpenDeck = false;
                }

                reuseTimer = reuseTime;
            }
        }
    }

    private void CardShow_OnSelectButtonClicked(object sender, CardShow.OnSelectButtonClickedEventArgs e)
    {
        cardsManagerUI.AddCard(e.cardSO);
        canOpenDeck = true;
        HideCards();
    }

    private void Update()
    {
        if (canOpenDeck) reuseTimer -= Time.deltaTime;
        timeToReuseFillBar.fillAmount = 1 - (reuseTimer / reuseTime);
    }

    private CardSO SelectCard(List<CardSO> cardSOList)
    {
        //There will be an error for now cause cards with different rarity do not exist
        //Once we add enough card the error should be fixed
        Dictionary<int, int> probabililities = new Dictionary<int, int> 
        {
            //These are probability for each value i.e., 65% chances for common, 30% for uncommon and 5% for rare
            //Value should add up to 100 if we decide to tweak the rarity factors
            {0, 65 }, //Common
            {1, 30 }, //Uncommon
            {2, 5 } //Rare
        };
        Dictionary<int, int> cumulativeProbabilities = new Dictionary<int, int>();
        int cumulativeProbabilityValue = 0;

        foreach(KeyValuePair<int, int> rarityProbability in probabililities)
        {
            cumulativeProbabilityValue += rarityProbability.Value;
            cumulativeProbabilities[rarityProbability.Key] = cumulativeProbabilityValue;
        }

        int randomNumber = Random.Range(0, 100);

        foreach(KeyValuePair<int, int> cumulativeProbability in cumulativeProbabilities)
        {
            if (randomNumber <= cumulativeProbability.Value)
            {
                List<CardSO> filteredList = cardSOList.Where(cardSO => (int)cardSO.rarity == cumulativeProbability.Key).ToList();
                if (filteredList.Count > 0)
                {
                    int randomFromFiltered = Random.Range(0, filteredList.Count);
                    return filteredList[randomFromFiltered];
                }
            }
        }
        return null;
    }
    private void HideCards()
    {
        foreach (CardShow cardShow in cardShows)
        {
            if (cardShow.gameObject.activeSelf) cardShow.gameObject.SetActive(false);
        }
    }
    public List<CardSO> GetAvailableCardsList() => availabeCardsList;
}
