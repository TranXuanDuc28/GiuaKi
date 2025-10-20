using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    public GameObject prefab;
    public float spawnTimer;
    public float spawnInterval;
    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime * GameManager.Instance.worldSpeed;
        if (spawnTimer >= spawnInterval)
        {
            SpawnObject();
            spawnTimer = 0f;
        }
        
    }

    private void SpawnObject()
    {
        Instantiate(prefab, RandomSpawnPoint(), transform.rotation);
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;

        spawnPoint.x = minPos.position.x;
        spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);

        return spawnPoint;
    }
}
