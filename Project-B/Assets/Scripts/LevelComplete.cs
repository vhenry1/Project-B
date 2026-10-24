using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
	[SerializeField] private TMP_Text completionTimeText;

	private void Start()
	{
		if (completionTimeText == null)
		{
			Debug.LogError("LevelComplete requires a completion time text reference.", this);
			return;
		}

		completionTimeText.text = $"Level Completed in {ScoreManager.CurrentTime:F2} seconds";
	}
	public void Next()
	{
		SceneManager.LoadScene("LevelSelect");
	}
}
