using UnityEngine;

public class LevelSpin : MonoBehaviour
{
    [SerializeField] private StatsHandler statsHandler;
    [SerializeField] private float generalOutRangeRotationSpeed;
    [SerializeField] private float generalInRangeRotationSpeed;
    [SerializeField] private Transform centrePoint;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float range;

    private float horizontalInput;
    private float verticalInput;

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 && verticalInput != 0 && Input.GetMouseButton(0)) return;

        if (playerPosition.position.x > (centrePoint.position.x + range)
            || playerPosition.position.x < (centrePoint.position.x - range))
        {
            transform.Rotate(new Vector3(0, horizontalInput * generalOutRangeRotationSpeed * statsHandler.moveSpeedMultiplier, 0));
        }
        else
        {
            transform.Rotate(new Vector3(0, horizontalInput * generalInRangeRotationSpeed * statsHandler.moveSpeedMultiplier, 0));
        }
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
