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
    public int scoreGoal = 20; // example goal to win
    public bool isGameState = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // make sure the game is running at normal time on start
            Time.timeScale = 1f;
    
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetWorldSpeed(float speed)
    {
        worldSpeed = speed;
    }

    public void AddScore(int points)
    {
        if (isGameState) return;
        score += points;
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
        }
        else
        {
            UIController.Instance.pauseMenu.SetActive(false);
            Time.timeScale = 1f;
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

    public void GameResult(bool isWin)
    {
        StartCoroutine(RestartGameCoroutine(isWin));
    }

    IEnumerator RestartGameCoroutine(bool isWin)
    {
       
        if(isGameState == false)  yield return new WaitForSecondsRealtime(2f);;
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetString("GameResult", isWin ? "Win" : "Lose");
        SceneManager.LoadScene("GameOver");
        Time.timeScale = 1f;
    }

}
