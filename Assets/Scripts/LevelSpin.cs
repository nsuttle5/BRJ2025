using UnityEngine;

public class LevelSpin : MonoBehaviour
{
    [SerializeField] private float generalOutRangeRotationSpeed;
    [SerializeField] private float generalInRangeRotationSpeed;
    [SerializeField] private Transform centrePoint;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float range;

    private float deltaPositionX;
    private float previousPositionX;

    private float horizontalInput;

    private void Awake()
    {
        deltaPositionX = 0;
        previousPositionX = playerPosition.position.x;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (playerPosition.position.x > (centrePoint.position.x + range)
            || playerPosition.position.x < (centrePoint.position.x - range))
        {
            transform.Rotate(new Vector3(0, horizontalInput * generalOutRangeRotationSpeed, 0));
        }
        else
        {
            transform.Rotate(new Vector3(0, horizontalInput * generalInRangeRotationSpeed, 0));
        }

        deltaPositionX = playerPosition.position.x - previousPositionX;
        previousPositionX = playerPosition.position.x;

    }

    private void OnDrawGizmos()
    {
        if (centrePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(centrePoint.position, new Vector3(centrePoint.position.x + range, centrePoint.position.y, centrePoint.position.z));
            Gizmos.DrawLine(centrePoint.position, new Vector3(centrePoint.position.x - range, centrePoint.position.y, centrePoint.position.z));
        }
    }
}
