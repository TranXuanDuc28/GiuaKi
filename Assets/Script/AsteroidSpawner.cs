using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;
    [SerializeField] private GameObject asteroidPrefab;

    private float spawnTimer;
    public float spawnInterval = 1.5f;  // Spawn thiên thạch nhanh hơn
    public float objectSpeedMultiplier = 1f;

    void Start()
    {
        if (asteroidPrefab == null)
        {
            Debug.LogError("ERROR: Chưa gán Asteroid Prefab! Hãy kéo Asteroid Prefab vào Inspector.", this);
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
            SpawnAsteroid();
            spawnTimer = 0f;
        }
    }

    private void SpawnAsteroid()
    {
        Vector2 spawnPoint = new Vector2(
            minPos.position.x,
            Random.Range(minPos.position.y, maxPos.position.y)
        );
        
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPoint, transform.rotation);
        
        // Áp dụng tốc độ từ level
        var rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity *= objectSpeedMultiplier;
        }
    }
}