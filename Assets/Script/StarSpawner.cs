using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;
    [SerializeField] private GameObject starPrefab;

    private float spawnTimer;
    public float spawnInterval = 3f;  // Thời gian spawn mặc định dài hơn

    void Start()
    {
        if (starPrefab == null)
        {
            Debug.LogError("ERROR: Chưa gán Star Prefab! Hãy kéo Star Prefab vào Inspector.", this);
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // Chỉ spawn sao khi đạt đến thời gian và dựa vào xác suất
        spawnTimer += Time.deltaTime * GameManager.Instance.worldSpeed;
        if (spawnTimer >= spawnInterval)
        {
            // Kiểm tra xác suất spawn sao từ LevelManager
            float starChance = LevelManager.Instance != null ? 
                LevelManager.Instance.GetCurrentLevelData().starSpawnChance : 
                GameConfig.LevelConfig.BASE_STAR_SPAWN_CHANCE;

            if (Random.value < starChance)
            {
                SpawnStar();
            }
            spawnTimer = 0f;
        }
    }

    private void SpawnStar()
    {
        Vector2 spawnPoint = new Vector2(
            minPos.position.x,
            Random.Range(minPos.position.y, maxPos.position.y)
        );
        
        Instantiate(starPrefab, spawnPoint, transform.rotation);
    }
}