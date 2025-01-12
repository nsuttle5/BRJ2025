using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [System.Serializable]
    public class Pool {
        public ProjectileTypes projectileType;
        public GameObject prefab;
        public int size;
    }
    public enum ProjectileTypes {
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
    [SerializeField] private Transform firePointCrouched;
    [Space(5)]

    [SerializeField] private float fireRate;

    private PlayerMovement playerMovement;

    private float fireRateTimer;
    private bool fireButtonPressed = false;
    private Vector2 moveDirection;
    private ProjectileTypes currentProjectileType;
    private Transform currentFirePoint;

    [SerializeField] private List<Pool> objectPoolList;
    private Dictionary<ProjectileTypes, Queue<GameObject>> projectilePoolDictionary;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        fireRateTimer = fireRate;
        currentProjectileType = ProjectileTypes.Bullets;
    }

    private void Start() {
        InputManager.Instance.OnFirePreformed += Instance_OnFirePreformed;
        InputManager.Instance.OnFireReleased += Instance_OnFireReleased;

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
    private void Instance_OnFireReleased(object sender, System.EventArgs e) {
        fireButtonPressed = false;
    }
    private void Instance_OnFirePreformed(object sender, System.EventArgs e) {
        fireButtonPressed = true;
    }

    private void Update() {
        fireRateTimer -= Time.deltaTime;
        moveDirection = playerMovement.GetMovementDirection();

        if (fireButtonPressed) {
            if (fireRateTimer <= 0 && currentProjectileType != ProjectileTypes.Fireball) {
                if (playerMovement.IsPositionLocked()) {
                    if (moveDirection.x == 0) {
                        currentFirePoint = (moveDirection.y > 0)
                            ? firePointUp : (moveDirection.y < 0)
                            ? firePointDown : firePointForward;
                    }
                    else {
                        currentFirePoint = (moveDirection.y > 0)
                            ? firePointDiagonalUp : (moveDirection.y < 0)
                            ? firePointDiagonalDown : firePointForward;
                    }
                }
                else {
                    currentFirePoint = (moveDirection.y > 0)
                        ? firePointUp : (moveDirection.y < 0)
                        ? firePointCrouched : firePointForward;
                }
                SpawnFromPool(currentProjectileType, currentFirePoint.position, currentFirePoint.rotation);
                fireRateTimer = fireRate;
            }
            else if (currentProjectileType == ProjectileTypes.Fireball) {

            }
        }
    }

    private GameObject SpawnFromPool(ProjectileTypes projectileType, Vector3 position, Quaternion rotation) {
        GameObject objectToSpawn = projectilePoolDictionary[projectileType].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        if (objectToSpawn.TryGetComponent(out IObjectPool pool)) {
            pool.SpawnObject();
        }

        projectilePoolDictionary[projectileType].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
