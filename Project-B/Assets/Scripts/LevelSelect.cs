using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private void Start()
    {
        ShowCompletionTimes();
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void LoadLevel2()
    {
        if (!HasCompletedLevel("Level1"))
        {
            return;
        }

        SceneManager.LoadScene("Level2");
    }
    public void LoadLevel3()
    {
        if (!HasCompletedLevel("Level1") || !HasCompletedLevel("Level2"))
        {
            return;
        }

        SceneManager.LoadScene("Level3");
    }

    private static void ShowCompletionTimes()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button button in buttons)
        {
            TMP_Text buttonLabel = button.GetComponentInChildren<TMP_Text>();
            if (buttonLabel == null || !TryGetLevelName(buttonLabel.text, out string sceneName))
            {
                continue;
            }

            bool isUnlocked = IsLevelUnlocked(sceneName);
            button.interactable = isUnlocked;

            TMP_Text timeLabel = Instantiate(buttonLabel, button.transform.parent, false);
            timeLabel.name = sceneName + " Completion Time";
            timeLabel.raycastTarget = false;

            RectTransform buttonRect = button.GetComponent<RectTransform>();
            RectTransform timeRect = timeLabel.rectTransform;
            timeRect.anchorMin = buttonRect.anchorMin;
            timeRect.anchorMax = buttonRect.anchorMax;
            timeRect.pivot = buttonRect.pivot;
            timeRect.anchoredPosition = buttonRect.anchoredPosition + Vector2.down * (buttonRect.sizeDelta.y + 4f);
            timeRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, 24f);
            timeRect.localScale = Vector3.one;
            timeLabel.fontSize = 14f;
            timeLabel.alignment = TextAlignmentOptions.Center;
            if (!isUnlocked)
            {
                timeLabel.text = sceneName == "Level2"
                    ? "Complete Level 1 to unlock"
                    : "Complete Levels 1 and 2 to unlock";
            }
            else
            {
                timeLabel.text = ScoreManager.TryGetLastCompletionTime(sceneName, out float completionTime)
                    ? $"Last: {completionTime:F2}s"
                    : "Not completed";
            }
        }
    }

    private static bool IsLevelUnlocked(string sceneName)
    {
        switch (sceneName)
        {
            case "Level1":
                return true;
            case "Level2":
                return HasCompletedLevel("Level1");
            case "Level3":
                return HasCompletedLevel("Level1") && HasCompletedLevel("Level2");
            default:
                return false;
        }
    }

    private static bool HasCompletedLevel(string sceneName)
    {
        return ScoreManager.TryGetLastCompletionTime(sceneName, out _);
    }

    private static bool TryGetLevelName(string buttonLabel, out string sceneName)
    {
        switch (buttonLabel.Trim())
        {
            case "Level 1":
                sceneName = "Level1";
                return true;
            case "Level 2":
                sceneName = "Level2";
                return true;
            case "Level 3":
                sceneName = "Level3";
                return true;
            default:
                sceneName = null;
                return false;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
