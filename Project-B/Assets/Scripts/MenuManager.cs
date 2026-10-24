using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        ScoreManager.ResetLevelProgress();
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
    public void Customization()
    {
        // Load the Customization scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Customization");
    }
    public void BackToMenu()
    {
        // Load the Main Menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
   
}
