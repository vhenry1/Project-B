using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
