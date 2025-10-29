using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float worldSpeed;
    // scoring
    public int score = 0;
    private int currentLevel;
    // Sử dụng scoreGoal ưu tiên từ LevelManager (Inspector) nếu có, ngược lại tính từ GameConfig
    public int scoreGoal
    {
        get
        {
            if (LevelManager.Instance != null)
            {
                var data = LevelManager.Instance.GetCurrentLevelData();
                if (data != null && data.scoreGoal > 0)
                    return data.scoreGoal;
            }
            return GameConfig.LevelConfig.BASE_SCORE_GOAL + (currentLevel - 1) * GameConfig.LevelConfig.SCORE_GOAL_INCREASE_PER_LEVEL;
        }
    }

    public bool isGameState = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            // make sure the game is running at normal time on start
            Time.timeScale = 1f;

        }
        else
        {
            Destroy(gameObject);
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "GameOver")
        {
            Destroy(gameObject);
        }
         // When gameplay scene loads, start gameplay music if available
        if (scene.name == "SpaceshipExplore")
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGameplayMusic();
            }
        }

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void SetWorldSpeed(float speed)
    {
        worldSpeed = speed;
    }

    public void AddScore(int points)
    {
        if (isGameState) return;
        score += points;
        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateScoreText(score, scoreGoal);
        }
        if (score >= scoreGoal)
        {   isGameState = true;
            GameResult(true);
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3"))
        {
            PauseGame();
        }   
    }
    public void PauseGame()
    {
        if (UIController.Instance.pauseMenu.activeSelf == false)
        {
            UIController.Instance.pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            // Play menu music when pause menu opens
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMenuMusic();
        }
        else
        {
            UIController.Instance.pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            // Stop menu music when resuming game
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopMusic();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetToLevel1()
    {
        // Reset các giá trị về mặc định
        currentLevel = 1;
        score = 0;
        isGameState = false;
        
        // Lưu level 1 vào PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("FinalScore", 0);
        
        // Reload scene hiện tại để bắt đầu lại
        SceneManager.LoadScene("SpaceshipExplore");
        Time.timeScale = 1f; // Đảm bảo game không bị pause
    }

    public void GameResult(bool isWin)
    {
        StartCoroutine(RestartGameCoroutine(isWin));
    }

    IEnumerator RestartGameCoroutine(bool isWin)
    {
        if(isGameState == false) yield return new WaitForSecondsRealtime(2f);
        
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetString("GameResult", isWin ? "Win" : "Lose");
        PlayerPrefs.SetInt("ScoreGoal", scoreGoal);
        
        if (isWin)
        {
            currentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }
        
        SceneManager.LoadScene("GameOver");
        Time.timeScale = 1f;
    }

}
