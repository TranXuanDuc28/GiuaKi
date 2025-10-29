using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    [Header("Spawn Settings")]
    [Tooltip("Kéo Prefab thiên thạch vào đây")]
    [SerializeField] private GameObject asteroidPrefab;
    [Tooltip("Kéo Prefab ngôi sao vào đây")]
    [SerializeField] private GameObject starPrefab;

    private float spawnTimer;
    
    [Header("Level Settings")]
    [Tooltip("Thời gian giữa mỗi lần spawn (được điều chỉnh bởi LevelManager)")]
    public float spawnInterval = 1.5f;
    [Tooltip("Hệ số nhân tốc độ (được điều chỉnh bởi LevelManager)")]
    public float objectSpeedMultiplier = 1f;

    void Start()
    {
        // Kiểm tra và cảnh báo nếu chưa gán prefab
        if (asteroidPrefab == null)
        {
            Debug.LogError("ERROR: Chưa gán prefab thiên thạch! Hãy kéo Asteroid Prefab vào Inspector.", this);
            enabled = false;
            return;
        }

        if (starPrefab == null)
        {
            Debug.LogError("ERROR: Chưa gán prefab ngôi sao! Hãy kéo Star Prefab vào Inspector.", this);
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        spawnTimer += Time.deltaTime * GameManager.Instance.worldSpeed;
        if (spawnTimer >= spawnInterval)
        {
            SpawnObject();
            spawnTimer = 0f;
        }
    }

    private void SpawnObject()
    {
        // Lấy thông tin level hiện tại
        float starChance = LevelManager.Instance != null ? 
            LevelManager.Instance.GetCurrentLevelData().starSpawnChance : 
            GameConfig.LevelConfig.BASE_STAR_SPAWN_CHANCE;

        // Quyết định xem có spawn sao hay không
        bool shouldSpawnStar = Random.value < starChance;

        // Chọn prefab dựa vào xác suất
        GameObject prefabToSpawn = shouldSpawnStar ? starPrefab : asteroidPrefab;

        GameObject spawnedObject = Instantiate(prefabToSpawn, RandomSpawnPoint(), transform.rotation);

        // Tùy chỉnh tốc độ của đối tượng nếu cần
        // Giả sử đối tượng có component Rigidbody2D hoặc một script điều khiển tốc độ
        var rb = spawnedObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Sử dụng linearVelocity thay cho velocity đã lỗi thời
            rb.linearVelocity *= objectSpeedMultiplier;
        }
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;
        spawnPoint.x = minPos.position.x;
        spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
        return spawnPoint;
    }
}
