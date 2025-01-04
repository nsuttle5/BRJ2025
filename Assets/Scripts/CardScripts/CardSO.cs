using UnityEngine;

[CreateAssetMenu(menuName = "ScritableObjects/Card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite cardImage;
    public GameObject cardPrefab;
    public Rarity rarity;

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        Rare
    }
}
