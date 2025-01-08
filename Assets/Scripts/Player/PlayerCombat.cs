using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ProjectileTypes projectileType;
        public GameObject prefab;
        public int size;
    }
    public enum ProjectileTypes
    {
        Bullets,
        Fireball,
        RichochetBullets,
    }

    [SerializeField] private StatsHandler statsHandler;

    [Header("Fire Points")]
    [SerializeField] private Transform firePointForward;
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointDiagonalUp;
    [SerializeField] private Transform firePointDiagonalDown;
    [SerializeField] private Transform firePointDown;
    [Space(5)]

    [SerializeField] private float fireRate;

    private PlayerMovement playerMovement;

    private float fireRateTimer;
    private bool canFire = true;
    private Vector2 moveDirection;
    private bool isGrounded;
    private ProjectileTypes currentProjectileType;
    private Transform currentFirePoint;

    [SerializeField] private List<Pool> objectPoolList;
    private Dictionary<ProjectileTypes, Queue<GameObject>> projectilePoolDictionary;

    private void Awake()
    {
        canFire = true;
        playerMovement = GetComponent<PlayerMovement>();
        fireRateTimer = fireRate;
        currentProjectileType = ProjectileTypes.Bullets;
    }

    private void Start()
    {
        projectilePoolDictionary = new Dictionary<ProjectileTypes, Queue<GameObject>>();

        foreach (Pool pool in objectPoolList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            projectilePoolDictionary.Add(pool.projectileType, objectPool);
        }
    }

    private void Update()
    {
        fireRateTimer -= Time.deltaTime;
        moveDirection = playerMovement.GetMovementDirection();
        isGrounded = playerMovement.GetIsGrounded();

        if (Input.GetMouseButton(0) && canFire)
        {
            if (fireRateTimer <= 0 && currentProjectileType != ProjectileTypes.Fireball)
            {
                if (playerMovement.IsFiringDiagonal())
                {
                    currentFirePoint = (moveDirection.y > 0)
                        ? firePointDiagonalUp
                        : firePointDiagonalDown;
                }
                else if (!isGrounded)
                {
                    currentFirePoint = (moveDirection.y > 0)
                        ? firePointUp : (moveDirection.y < 0)
                        ? firePointDown : firePointForward;
                }
                else
                {
                    currentFirePoint = (moveDirection.y > 0)
                        ? firePointUp : firePointForward;
                }
                SpawnFromPool(currentProjectileType, currentFirePoint.position, currentFirePoint.rotation);
                fireRateTimer = fireRate;
            }
            else if (currentProjectileType == ProjectileTypes.Fireball)
            {

            }
        }
    }

    private GameObject SpawnFromPool(ProjectileTypes projectileType, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = projectilePoolDictionary[projectileType].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        if (objectToSpawn.TryGetComponent(out IObjectPool pool))
        {
            pool.SpawnObject();
        }

        projectilePoolDictionary[projectileType].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
