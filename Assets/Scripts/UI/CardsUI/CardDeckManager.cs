using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardDeckManager : MonoBehaviour
{
    [Header("Cardpack Settings")]
    [SerializeField] private Transform[] cardpackSlots;
    [SerializeField] private List<CardSO> cardList;
    [SerializeField] private Button openDeckButton;

    private bool canOpenDeck = true;

    private void Awake()
    {
        openDeckButton.onClick.AddListener(() =>
        {
            if (canOpenDeck)
            {
                for (int i = 0; i < cardpackSlots.Length; i++)
                {
                    //Select 3 card from the list (Also consider the rarity value)
                    CardSO selectedCard = SelectCard(cardList);

                    //From the selected 3 card place each of them on the card slot pack
                    GameObject card = Instantiate(selectedCard.cardPrefab, cardpackSlots[i], false);
                    card.transform.localScale = new Vector3(2, 2, 2);

                    canOpenDeck = false;
                }
            }
        });
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
                int randomFromFiltered = Random.Range(0, filteredList.Count);
                return filteredList[randomFromFiltered];
            }
        }

        return null; //Should not reach here if reached then there is a problem with code;
    }
}
