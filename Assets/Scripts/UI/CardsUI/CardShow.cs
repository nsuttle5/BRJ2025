using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardShow : MonoBehaviour
{
    public event EventHandler<OnSelectButtonClickedEventArgs> OnSelectButtonClicked;
    public class OnSelectButtonClickedEventArgs : EventArgs
    {
        public CardSO cardSO;
    }

    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescriptionText;
    public Image cardImage;
    public TextMeshProUGUI rarityText;
    public Button selectButton;

    [HideInInspector] public CardSO cardSO;

    private void Awake()
    {
        selectButton.onClick.AddListener(() =>
        {
            if (cardSO != null)
            {
                OnSelectButtonClicked?.Invoke(this, new OnSelectButtonClickedEventArgs
                {
                    cardSO = cardSO
                });
            }
        });
    }
}
