using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
	private const string BestTimeKeyPrefix = "BestCompletionTime_";

	private static ScoreManager instance;
	private string levelName;
	private float startTime;
	private float completedTime;
	private bool isTiming;

	public static float CurrentTime => instance == null
		? 0f
		: instance.isTiming ? Time.time - instance.startTime : instance.completedTime;

	public static bool IsTiming => instance != null && instance.isTiming;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void EnsureInstance()
	{
		if (instance == null)
		{
			new GameObject(nameof(ScoreManager)).AddComponent<ScoreManager>();
		}
	}

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
		BeginTimingForScene(SceneManager.GetActiveScene());
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			instance = null;
		}
	}

	private void Update()
	{
		if (isTiming && FindObjectsByType<PeopleScript>(FindObjectsSortMode.None).Length == 0)
		{
			CompleteLevel();
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "LevelComplete")
		{
			return;
		}

		BeginTimingForScene(scene);
	}

	private void BeginTimingForScene(Scene scene)
	{
		isTiming = false;
		completedTime = 0f;
		levelName = scene.name;

		PeopleScript[] people = FindObjectsByType<PeopleScript>(FindObjectsSortMode.None);
		if (people.Length == 0)
		{
			return;
		}

		SpawnRandomPeople(people);
		startTime = Time.time;
		isTiming = true;
	}

	private static void SpawnRandomPeople(PeopleScript[] people)
	{
		int minimumSpawnCount = Mathf.Min(2, people.Length);
		int spawnCount = Random.Range(minimumSpawnCount, people.Length + 1);

		for (int i = 0; i < people.Length; i++)
		{
			int randomIndex = Random.Range(i, people.Length);
			(people[i], people[randomIndex]) = (people[randomIndex], people[i]);
		}

		for (int i = 0; i < spawnCount; i++)
		{
			PeopleScript template = people[i];
			Instantiate(template.gameObject, template.transform.position, template.transform.rotation);
		}

		foreach (PeopleScript person in people)
		{
			person.gameObject.SetActive(false);
			Destroy(person.gameObject);
		}
	}

	private void CompleteLevel()
	{
		completedTime = Time.time - startTime;
		isTiming = false;

		string key = BestTimeKeyPrefix + levelName;
		if (!PlayerPrefs.HasKey(key) || completedTime < PlayerPrefs.GetFloat(key))
		{
			PlayerPrefs.SetFloat(key, completedTime);
			PlayerPrefs.Save();
		}

		SceneManager.LoadScene("LevelComplete");
	}

	public static bool TryGetBestTime(string sceneName, out float bestTime)
	{
		string key = BestTimeKeyPrefix + sceneName;
		if (PlayerPrefs.HasKey(key))
		{
			bestTime = PlayerPrefs.GetFloat(key);
			return true;
		}

		bestTime = 0f;
		return false;
	}
}
