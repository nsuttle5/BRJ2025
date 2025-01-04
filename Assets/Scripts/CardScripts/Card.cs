using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private CardSO cardSO;

    private void Awake()
    {
        image.sprite = cardSO.cardImage;
    }

    public void SetImageActive(bool condition)
    {
        image.enabled = condition;
    }


    public abstract void UseCard();
}
