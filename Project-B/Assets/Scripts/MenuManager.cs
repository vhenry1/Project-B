using UnityEngine;

public class MenuManager : MonoBehaviour
{
   

    
    public void StartGame()
    {
        // Load the Level select
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }
    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }

    public void Credits()
    {
        // Load the Credits scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
   
}
