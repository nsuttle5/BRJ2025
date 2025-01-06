using UnityEngine;

public class TheSunCard : Card
{
    [SerializeField] private GameObject fireballPrefab;
    public override void UseCard()
    {
        Transform firePoint = PlayerManager.Instance.GetFirePoint();
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        Destroy(gameObject);
    }
}
