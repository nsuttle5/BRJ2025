using System.Collections;
using UnityEngine;

public class CouroutineHandler : MonoBehaviour
{
    public static CouroutineHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private IEnumerator DebuffFoolCard()
    {
        yield return new WaitForSeconds(1);//Ignore this script for now
    }
}
