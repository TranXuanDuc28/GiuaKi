
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour 
{

   void Start()
{
    Time.timeScale = 1f;
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlayMenuMusic();
    }
    else
    {
        Debug.LogWarning("MenuManager.Start: AudioManager.Instance is null, cannot play music yet!");
    }
}

    public void NewGameFunction()
    {
        Debug.Log("MenuManager.NewGameFunction called. GameManager.Instance exists: " + (GameManager.Instance != null));
        // ensure we set world speed after the new scene is loaded (GameManager may not exist yet)
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Stop menu music when starting a new game
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        // if a GameManager already exists (e.g. persistent), reset score and UI immediately
        if (GameManager.Instance != null)
        {
            GameManager.Instance.score = 0;
            GameManager.Instance.isGameState = false;
            if (UIController.Instance != null)
                UIController.Instance.UpdateScoreText(0, GameManager.Instance.scoreGoal);
        }
        SceneManager.LoadScene("SpaceshipExplore");

    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log($"MenuManager.OnSceneLoaded: scene={scene.name} mode={mode}");
        if (scene.name == "SpaceshipExplore")
        {
            Debug.Log("Loaded SpaceshipExplore - GameManager.Instance=" + (GameManager.Instance != null));
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetWorldSpeed(1f);
                // reset score state on newly loaded scene as well
                GameManager.Instance.score = 0;
                GameManager.Instance.isGameState = false;
            }
            if (UIController.Instance != null && GameManager.Instance != null)
            {
                UIController.Instance.UpdateScoreText(GameManager.Instance.score, GameManager.Instance.scoreGoal);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else if (scene.name == "MainMenu")
        {
            Debug.Log("Loaded MainMenu - scheduling EnsureMenuInteractive");
            // run one frame later to avoid race with UIController/Awake in the menu scene
            StartCoroutine(EnsureMenuInteractive());
        }
    }

    private System.Collections.IEnumerator EnsureMenuInteractive()
    {
        yield return null; // wait one frame
        Time.timeScale = 1f;

        // ensure EventSystem exists and is enabled
        if (EventSystem.current == null)
        {
            GameObject esGO = new GameObject("EventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<StandaloneInputModule>();
            Debug.Log("MenuManager: created EventSystem at runtime");
        }
        else
        {
            EventSystem.current.enabled = true;
            Debug.Log("MenuManager: EventSystem exists; enabled=" + EventSystem.current.enabled);
        }

        // try hide pause menu: prefer UIController if present, otherwise find by name
        if (UIController.Instance != null && UIController.Instance.pauseMenu != null)
        {
            UIController.Instance.pauseMenu.SetActive(false);
            Debug.Log("MenuManager: hid pauseMenu via UIController instance");
        }
        else
        {
            var pm = GameObject.Find("PauseMenu");
            if (pm != null) pm.SetActive(false);
            Debug.Log("MenuManager: hid pauseMenu by name? " + (GameObject.Find("PauseMenu") != null));
        }

        // Play menu music when main menu is visible
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }

        Debug.Log($"MenuManager.EnsureMenuInteractive: Time.timeScale={Time.timeScale}, GameManager.Instance={(GameManager.Instance!=null)}, UIController.Instance={(UIController.Instance!=null)}");

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void ExitGameFunction()
    {
        Application.Quit();
    }

}
