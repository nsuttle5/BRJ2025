using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    [HideInInspector] public CardSO cardSO;

    public bool isSelected;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (isSelected) outline.enabled = true;
        else outline.enabled = false;
    }

    public void MouseHover()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    public void MouseDeHover()
    {
        transform.localScale = Vector3.one;
    }
    public void MouseClick()
    {
        if (!isSelected && CardInventoryManager.Instance.CanEquipMoreCards())
        {
            if (CardInventoryManager.Instance.GetEquippedCards().Contains(cardSO)) 
                CardInventoryManager.Instance.GetEquippedCards().Add(cardSO);

            outline.enabled = true;
            isSelected = true;
        }
        else
        {
            if (CardInventoryManager.Instance.GetEquippedCards().Contains(cardSO)) 
                CardInventoryManager.Instance.GetEquippedCards().Remove(cardSO);

            outline.enabled = false;
            isSelected = false;
        }
    }
}
