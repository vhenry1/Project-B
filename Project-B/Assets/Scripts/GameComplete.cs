using UnityEngine;
using TMPro;

public class GameComplete : MonoBehaviour
{
	[SerializeField] private TMP_Text totalTimeText;

	private void Start()
	{
		if (totalTimeText == null)
		{
			Debug.LogError("GameComplete requires a total time text reference.", this);
			return;
		}

		if (!ScoreManager.TryGetTotalCompletionTime(out float totalCompletionTime))
		{
			Debug.LogError("No total completion time is available for GameComplete.", this);
			return;
		}

		totalTimeText.rectTransform.sizeDelta = new Vector2(600f, 80f);
		totalTimeText.fontSize = 32f;
		totalTimeText.text = $"Total time: {totalCompletionTime:F2} seconds";
	}
    public void BackToMenu()
    {
        // Load the Main Menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
