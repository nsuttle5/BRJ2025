using UnityEngine;

public class MissileLauncher : MonoBehaviour {
    [Header("Spawn Settings")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float arcAngle = 180f;  // Half moon = 180 degrees
    [SerializeField] private float minMissileSpacing = 1f;  // Minimum distance between missiles

    [Header("Spawn Position")]
    [SerializeField] private float heightOffset = 2f;  // How high above the spawner to place missiles
    [SerializeField] private bool visualizeSpawnPoints = true;

    [Header("Spawn Timing")]
    [SerializeField] private float delayBetweenSpawns = 0.1f;
    private float nextSpawnTime;
    private int missileCount = 0;

    // Cache for spawn positions
    private Vector3[] spawnPositions;

    public void LaunchMissiles(int count) {
        missileCount = count;
        CalculateSpawnPositions();
        SpawnMissileWave();
    }

    private void CalculateSpawnPositions()
    {
        // Calculate how many missiles we can actually fit given the spacing constraints
        float arcLength = (arcAngle / 360f) * 2f * Mathf.PI * spawnRadius;
        int maxMissiles = Mathf.FloorToInt(arcLength / minMissileSpacing);
        int actualMissileCount = Mathf.Min(missileCount, maxMissiles);

        spawnPositions = new Vector3[actualMissileCount];

        // Calculate angle between each missile
        float angleStep = arcAngle / (actualMissileCount - 1);
        float startAngle = -arcAngle / 2f;  // Center the arc

        for (int i = 0; i < actualMissileCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            float radians = currentAngle * Mathf.Deg2Rad;

            // Calculate position on the arc
            float x = Mathf.Sin(radians) * spawnRadius;
            float y = Mathf.Cos(radians) * spawnRadius;

            // Store position relative to spawner
            spawnPositions[i] = new Vector3(x, y + heightOffset, 0f);
        }
    }

    public void SpawnMissileWave()
    {
        StartCoroutine(SpawnMissilesSequentially());
    }

    private System.Collections.IEnumerator SpawnMissilesSequentially()
    {
        foreach (Vector3 spawnPos in spawnPositions)
        {
            SpawnSingleMissile(spawnPos);
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    private void SpawnSingleMissile(Vector3 spawnPosition)
    {
        // Convert local spawn position to world space
        Vector3 worldSpawnPos = transform.TransformPoint(spawnPosition);

        // Spawn the missile
        GameObject missile = Instantiate(missilePrefab, worldSpawnPos, Quaternion.identity);

        // Optional: You could initialize any missile-specific properties here
        // SeekerMissile seekerComponent = missile.GetComponent<SeekerMissile>();
        // if (seekerComponent != null)
        // {
        //     seekerComponent.Initialize();
        // }
    }

    private void OnDrawGizmosSelected()
    {
        if (!visualizeSpawnPoints) return;

        // Draw the spawn radius
        Gizmos.color = Color.yellow;

        // Draw arc
        int segments = 32;
        float angleStep = arcAngle / segments;
        float startAngle = -arcAngle / 2f;

        Vector3 previousPoint = Vector3.zero;
        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + (angleStep * i);
            float radians = angle * Mathf.Deg2Rad;

            Vector3 point = transform.position + new Vector3(
                Mathf.Sin(radians) * spawnRadius,
                Mathf.Cos(radians) * spawnRadius + heightOffset,
                0f
            );

            if (i > 0)
            {
                Gizmos.DrawLine(previousPoint, point);
            }
            previousPoint = point;
        }

        // Draw spawn positions if calculated
        if (spawnPositions != null)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 pos in spawnPositions)
            {
                Vector3 worldPos = transform.TransformPoint(pos);
                Gizmos.DrawWireSphere(worldPos, 0.2f);
            }
        }
    }

    // Public method to recalculate spawn positions if parameters change
    public void RecalculateSpawnPositions()
    {
        CalculateSpawnPositions();
    }

#if UNITY_EDITOR
    // Optional: Add a button in the inspector to test spawning
    [UnityEditor.CustomEditor(typeof(MissileLauncher))]
    public class MissileSpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MissileLauncher spawner = (MissileLauncher)target;

            if (GUILayout.Button("Recalculate Spawn Positions"))
            {
                spawner.RecalculateSpawnPositions();
            }

            if (GUILayout.Button("Spawn Missile Wave"))
            {
                spawner.SpawnMissileWave();
            }
        }
    }
#endif
}

