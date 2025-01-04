using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public int numberOfCards = 10; // Number of cards in the deck
    public Sprite[] cardBackDesigns; // Array of possible card back designs
    public Transform deckPosition; // Position where the deck will be instantiated
    public float cardStackOffset = 0.1f; // Offset for stacking cards

    [Header("Card Prefab")]
    public GameObject cardPrefab; // Prefab for the card object

    private Stack<GameObject> cardStack = new Stack<GameObject>(); // Stack to hold cards

    private void Start()
    {
        CreateDeck();
    }

    private void CreateDeck()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity, deckPosition);

            // Assign a random back design to the card
            SpriteRenderer spriteRenderer = newCard.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && cardBackDesigns.Length > 0)
            {
                spriteRenderer.sprite = cardBackDesigns[Random.Range(0, cardBackDesigns.Length)];
            }

            // Adjust the position for stacking effect
            Vector3 offset = new Vector3(0, i * cardStackOffset, 0);
            newCard.transform.position += offset;

            // Add card to the stack
            cardStack.Push(newCard);
        }
    }

    public void ShiftTopCard()
    {
        if (cardStack.Count > 0)
        {
            GameObject topCard = cardStack.Pop();

            // Play animation to shift the top card
            Animator animator = topCard.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Shift");
            }

            // Optionally destroy or handle the card after the animation
            Destroy(topCard, 1f); // Adjust delay based on animation duration
        }
    }
}

