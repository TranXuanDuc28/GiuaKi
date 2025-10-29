using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore");
        string result = PlayerPrefs.GetString("GameResult");
        // Prefer level-defined goal if LevelManager still available, otherwise fall back to saved PlayerPrefs
        int scoreGoal = 0;
        if (LevelManager.Instance != null)
        {
            var data = LevelManager.Instance.GetCurrentLevelData();
            if (data != null)
                scoreGoal = data.scoreGoal;
        }
        if (scoreGoal <= 0)
            scoreGoal = PlayerPrefs.GetInt("ScoreGoal", 100); // fallback

        scoreText.text = "Score: " + finalScore + "/" + scoreGoal;
            resultText.text = result == "Win" ? GameConfig.UIConfig.WIN_TEXT : GameConfig.UIConfig.LOSE_TEXT;        // Show/hide appropriate buttons based on result
        restartButton.gameObject.SetActive(result == "Lose");
        nextLevelButton.gameObject.SetActive(result == "Win");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SpaceshipExplore");
    }

    public void NextLevel()
    {
        // Level is already incremented in GameManager
        SceneManager.LoadScene("SpaceshipExplore");
    }
}