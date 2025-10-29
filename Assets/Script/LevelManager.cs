using System.Collections.Generic;
using UnityEngine;

// Định nghĩa cấu trúc dữ liệu cho mỗi level
[System.Serializable]
public class LevelData
{
    public string levelName;
    public float spawnInterval; // Tần suất spawn
    public float objectSpeedMultiplier; // Hệ số nhân tốc độ
    public int additionalAsteroidHealth; // Số máu thêm vào cho tiểu hành tinh
    public float starSpawnChance; // Tỉ lệ spawn sao
    public int scoreGoal; // Số điểm cần để qua màn (sao)

    // Constructor với giá trị từ GameConfig
    public LevelData(int level)
    {
        levelName = $"Level {level}";
        
        // Tính các giá trị dựa trên level từ GameConfig
        spawnInterval = GameConfig.LevelConfig.BASE_SPAWN_INTERVAL - 
                       ((level - 1) * GameConfig.LevelConfig.SPAWN_INTERVAL_DECREASE_PER_LEVEL);
        
        objectSpeedMultiplier = GameConfig.LevelConfig.BASE_SPEED_MULTIPLIER +
                               ((level - 1) * GameConfig.LevelConfig.SPEED_INCREASE_PER_LEVEL);
        
        // Tính số máu thêm vào dựa trên level
        additionalAsteroidHealth = (level - 1) / 2; // Cứ 2 level tăng 1 máu
        
        // Giảm tỉ lệ sao theo level nhưng không thấp hơn 10%
        starSpawnChance = Mathf.Max(0.1f, GameConfig.LevelConfig.BASE_STAR_SPAWN_CHANCE - 
                                         ((level - 1) * GameConfig.LevelConfig.STAR_CHANCE_DECREASE_PER_LEVEL));
    // Tính scoreGoal dựa trên cấu hình chung
    scoreGoal = GameConfig.LevelConfig.BASE_SCORE_GOAL + (level - 1) * GameConfig.LevelConfig.SCORE_GOAL_INCREASE_PER_LEVEL;
    }
    
    // Constructor mặc định cho Unity Inspector
    public LevelData()
    {
        levelName = "New Level";
        spawnInterval = GameConfig.LevelConfig.BASE_SPAWN_INTERVAL;
        objectSpeedMultiplier = GameConfig.LevelConfig.BASE_SPEED_MULTIPLIER;
        additionalAsteroidHealth = 0;
        starSpawnChance = GameConfig.LevelConfig.BASE_STAR_SPAWN_CHANCE;
        scoreGoal = GameConfig.LevelConfig.BASE_SCORE_GOAL;
    }
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [System.Serializable]
    public class LevelSettings
    {
        public string levelName = "New Level";
        [Header("Spawn Settings")]
        [Tooltip("Tần suất spawn gốc cho level này (giây)")]
        public float baseSpawnInterval = 3.0f;
        [Tooltip("Tốc độ di chuyển của các object (1 = bình thường)")]
        public float speedMultiplier = 1.0f;
        
        [Header("Thiên thạch")]
        [Tooltip("Số máu thêm vào cho thiên thạch")]
        public int additionalAsteroidHealth = 0;
        
        [Header("Sao")]
        [Range(0.1f, 1f)]
        [Tooltip("Tỉ lệ xuất hiện sao (0.1 = 10%, 1 = 100%)")]
        public float starSpawnChance = 0.25f;
        [Tooltip("Số sao cần thu thập để qua màn")]
        public int scoreGoal = 15;
    }

    [Header("Level Configuration")]
    [Tooltip("Thiết lập cho từng level")]
    public List<LevelSettings> levelConfigs = new List<LevelSettings>();

    [Header("Auto Generation")]
    [Tooltip("Tự động tạo thêm level nếu vượt quá số level đã cấu hình")]
    public bool autoGenerateNewLevels = true;

    private int currentLevel;
    private AsteroidSpawner[] asteroidSpawners;
    private StarSpawner[] starSpawners;
    private LevelData currentLevelData;

    // Thêm property để truy cập level hiện tại từ bên ngoài
    public int CurrentLevel => currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tìm tất cả spawner trong scene
        asteroidSpawners = FindObjectsByType<AsteroidSpawner>(FindObjectsSortMode.None);
        starSpawners = FindObjectsByType<StarSpawner>(FindObjectsSortMode.None);
        
        // Initialize current level and load settings early so other scripts (UI/GameManager) can read them in Awake
        if (levelConfigs.Count == 0)
        {
            levelConfigs.Add(new LevelSettings());
        }
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        LoadLevelSettings();
    }

    // Thêm phương thức để lấy thông tin level hiện tại
    public LevelData GetCurrentLevelData()
    {
        return currentLevelData;
    }

    private LevelSettings GenerateNewLevelSettings(int level)
    {
        // Lấy settings của level cuối cùng làm cơ sở
        LevelSettings lastSettings = levelConfigs[levelConfigs.Count - 1];
        LevelSettings newSettings = new LevelSettings
        {
            levelName = $"Level {level}",
            baseSpawnInterval = Mathf.Max(0.5f, lastSettings.baseSpawnInterval - 0.2f),
            speedMultiplier = lastSettings.speedMultiplier + 0.1f,
            additionalAsteroidHealth = lastSettings.additionalAsteroidHealth + ((level - levelConfigs.Count) / 2),
            starSpawnChance = Mathf.Max(0.1f, lastSettings.starSpawnChance - 0.02f),
            scoreGoal = lastSettings.scoreGoal + 5
        };
        
        // Thêm vào danh sách cấu hình
        levelConfigs.Add(newSettings);
        return newSettings;
    }

    void Start()
    {
        // Start is intentionally minimal because LoadLevelSettings is run in Awake to make data available early.
        // If for some reason currentLevelData wasn't initialized in Awake, ensure it's loaded here.
        if (currentLevelData == null)
        {
            if (levelConfigs.Count == 0)
                levelConfigs.Add(new LevelSettings());
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            LoadLevelSettings();
        }
    }

    void LoadLevelSettings()
    {
        // Đảm bảo có ít nhất một cấu hình level
        if (levelConfigs.Count == 0)
        {
            levelConfigs.Add(new LevelSettings());
        }

        // Lấy cấu hình level hiện tại
        LevelSettings settings;
        if (currentLevel <= levelConfigs.Count)
        {
            // Sử dụng cấu hình đã định nghĩa
            settings = levelConfigs[currentLevel - 1];
        }
        else if (autoGenerateNewLevels)
        {
            // Tự động tạo cấu hình mới dựa trên level cuối cùng
            settings = GenerateNewLevelSettings(currentLevel);
        }
        else
        {
            // Sử dụng cấu hình của level cuối cùng
            settings = levelConfigs[levelConfigs.Count - 1];
        }

        // Tạo LevelData từ settings
        currentLevelData = new LevelData
        {
            levelName = settings.levelName,
            spawnInterval = settings.baseSpawnInterval,
            objectSpeedMultiplier = settings.speedMultiplier,
            additionalAsteroidHealth = settings.additionalAsteroidHealth,
            starSpawnChance = settings.starSpawnChance,
            scoreGoal = settings.scoreGoal
        };
        
        Debug.Log($"Loading settings for {settings.levelName}:");
        Debug.Log($"- Spawn Interval: {settings.baseSpawnInterval:F2}s");
        Debug.Log($"- Speed Multiplier: {settings.speedMultiplier:F2}x");
        Debug.Log($"- Additional Health: +{settings.additionalAsteroidHealth}");
        Debug.Log($"- Star Spawn Chance: {settings.starSpawnChance:P0}");
        Debug.Log($"- Score Goal: {settings.scoreGoal}");

        // Áp dụng cài đặt cho asteroid spawners
        foreach (var spawner in asteroidSpawners)
        {
            spawner.spawnInterval = currentLevelData.spawnInterval;
            spawner.objectSpeedMultiplier = currentLevelData.objectSpeedMultiplier;
        }

        // Áp dụng cài đặt cho star spawners
        foreach (var spawner in starSpawners)
        {
            spawner.spawnInterval = currentLevelData.spawnInterval * 1.5f; // Star spawn chậm hơn
        }

        // Notify UI (if present) about the new goal so HUD shows correct value immediately
        if (UIController.Instance != null)
        {
            int currentScore = (GameManager.Instance != null) ? GameManager.Instance.score : 0;
            UIController.Instance.UpdateScoreText(currentScore, currentLevelData.scoreGoal);
        }
    }
}
