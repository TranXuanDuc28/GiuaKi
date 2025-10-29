using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text energyText;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TMP_Text experienceText;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text levelText; // Thêm text hiển thị level

    public GameObject pauseMenu;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // ensure the pause menu is hidden when the game starts
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        // initialize score display if possible: always prefer LevelManager's per-level goal
        if (scoreText != null)
        {
            int s = 0;
            int goal = 0;

            if (GameManager.Instance != null)
                s = GameManager.Instance.score;

            if (LevelManager.Instance != null)
            {
                var data = LevelManager.Instance.GetCurrentLevelData();
                if (data != null)
                    goal = data.scoreGoal;
            }

            // fallback to GameConfig if no level data available
            if (goal <= 0)
            {
                int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
                goal = GameConfig.LevelConfig.BASE_SCORE_GOAL + (currentLevel - 1) * GameConfig.LevelConfig.SCORE_GOAL_INCREASE_PER_LEVEL;
            }

            scoreText.text = s + " / " + goal;
        }
        // Hiển thị level
        UpdateLevelText();
    }

    public void UpdateEnergySlider(float current, float max)
    {
        // ensure slider max is set first, then assign a clamped integer value
        energySlider.maxValue = max;
        int cur = Mathf.Clamp(Mathf.RoundToInt(current), 0, Mathf.RoundToInt(max));
        energySlider.value = cur;
        energyText.text = cur + " / " + Mathf.RoundToInt(max);
    }
    public void UpdateHealthSlider(float current, float max)
    {
        // ensure slider max is set first, then assign a clamped integer value
        healthSlider.maxValue = max;
        int cur = Mathf.Clamp(Mathf.RoundToInt(current), 0, Mathf.RoundToInt(max));
        healthSlider.value = cur;
        healthText.text = cur + " / " + Mathf.RoundToInt(max);
    }
     public void UpdateExperienceSlider(float current, float max)
    {
        // ensure slider max is set first, then assign a clamped integer value
        experienceSlider.maxValue = max;
        int cur = Mathf.Clamp(Mathf.RoundToInt(current), 0, Mathf.RoundToInt(max));
        experienceSlider.value = cur;
        experienceText.text = cur + " / " + Mathf.RoundToInt(max);
    }

    public void UpdateScoreText(int score, int goal)
    {
        if (scoreText != null)
        {
            int displayGoal = goal;
            if (LevelManager.Instance != null)
            {
                var data = LevelManager.Instance.GetCurrentLevelData();
                if (data != null)
                    displayGoal = data.scoreGoal;
            }
            // fallback to passed goal if no level data
            scoreText.text = score + " / " + displayGoal;
        }
        
        // Cập nhật level mỗi khi cập nhật điểm
        UpdateLevelText();
    }

    public void UpdateLevelText()
    {
        if (levelText != null && GameManager.Instance != null)
        {
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            levelText.text = "Level: " + currentLevel;
        }
    }
}
