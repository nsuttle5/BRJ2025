using UnityEngine;

public class SimpleLevelSpinning : MonoBehaviour
{
    public float rotateSpeed = 0.5f;
    private float horizontalInput;
    
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        transform.Rotate(new Vector3(0, horizontalInput * rotateSpeed, 0));

    }
}
