
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void NewGameFunction()
    {
        SceneManager.LoadScene("SpaceshipExplore");
    }
    
    public void ExitGameFunction()
    {
        Application.Quit();
    }

}
