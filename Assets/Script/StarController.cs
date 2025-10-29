using UnityEngine;
using System;

[Serializable]
public class StarType
{
    public Sprite sprite;
    public int points = 1;
    public int experience = 1;
    public string starName = "Star"; // Tên hiển thị của sao (tùy chọn)
    public Color color = Color.white;
    [Range(0.3f, 3f)] public float size = 1f; // 🔹 Kích thước riêng cho mỗi loại sao
}

public class StarController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private StarType[] starTypes; // Mảng chứa thông tin của từng loại sao
    private StarType currentStar; // Thông tin của sao hiện tại
    private Vector3 targetPosition;
    private float moveSpeed;

    void Start()
    {
        if (starTypes == null || starTypes.Length == 0)
        {
            Debug.LogError("Chưa cấu hình StarTypes! Hãy thêm ít nhất một StarType trong Inspector.");
            Destroy(gameObject);
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        // 🔸 Chọn ngẫu nhiên một loại sao
        currentStar = starTypes[UnityEngine.Random.Range(0, starTypes.Length)];

        // 🔸 Áp dụng sprite
        spriteRenderer.sprite = currentStar.sprite;

        // 🔸 Áp dụng màu riêng
        spriteRenderer.color = currentStar.color;

        // 🔸 Random tốc độ di chuyển
        moveSpeed = UnityEngine.Random.Range(GameConfig.StarConfig.MIN_MOVE_SPEED, GameConfig.StarConfig.MAX_MOVE_SPEED);

        // 🔸 Tạo vị trí đích
        generateRandomPosition();

        // 🔸 Scale kích thước dựa trên cấu hình + thêm chút ngẫu nhiên
        float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
        float finalScale = currentStar.size * randomFactor;
        transform.localScale = new Vector3(finalScale, finalScale, 1f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        float moveX = GameManager.Instance.worldSpeed * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0f, 0f);

        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }

    private void generateRandomPosition()
    {
        float randomX = UnityEngine.Random.Range(GameConfig.StarConfig.MIN_SPAWN_X, GameConfig.StarConfig.MAX_SPAWN_X);
        float randomY = UnityEngine.Random.Range(GameConfig.StarConfig.MIN_SPAWN_Y, GameConfig.StarConfig.MAX_SPAWN_Y);
        targetPosition = new Vector3(randomX, randomY, 0f);
        moveSpeed = UnityEngine.Random.Range(GameConfig.StarConfig.MIN_TARGET_SPEED, GameConfig.StarConfig.MAX_TARGET_SPEED);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.GetExperience(currentStar.experience);
            GameManager.Instance.AddScore(currentStar.points);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCollectStar();
            }

            Debug.Log($"Collected {currentStar.starName} worth {currentStar.points} points!");
            Destroy(gameObject);
        }
    }
}
