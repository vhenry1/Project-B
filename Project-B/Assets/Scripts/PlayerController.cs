using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
	[SerializeField, Min(0f)] private float moveSpeed = 1f;
	[SerializeField, Min(0f)] private float respawnDelay = 0.5f;

	private static PlayerController instance;
	private Rigidbody2D body;
	private SpriteRenderer playerRenderer;
	private Sprite squareSprite;
	private Sprite circleSprite;
	private SpriteRenderer[] spriteRenderers;
	private Collider2D[] colliders;
	private bool[] spriteRendererStates;
	private bool[] colliderStates;
	private bool bodyWasSimulated;
	private bool isRespawning;
	private Vector2 movementInput;
	private Coroutine respawnRoutine;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
		body = GetComponent<Rigidbody2D>();
		body.gravityScale = 0f;
		body.freezeRotation = true;
		playerRenderer = GetComponent<SpriteRenderer>();
		squareSprite = playerRenderer.sprite;
		circleSprite = Customization.CreateCircleSprite(squareSprite);
		ApplyCustomization();
		bodyWasSimulated = body.simulated;
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
		colliders = GetComponentsInChildren<Collider2D>(true);
		spriteRendererStates = new bool[spriteRenderers.Length];
		colliderStates = new bool[colliders.Length];

		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			spriteRendererStates[i] = spriteRenderers[i].enabled;
		}

		for (int i = 0; i < colliders.Length; i++)
		{
			colliderStates[i] = colliders[i].enabled;
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Start()
	{
		if (IsLevelScene(SceneManager.GetActiveScene()))
		{
			BeginRespawn();
		}
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			instance = null;
		}

		if (circleSprite != null)
		{
			Destroy(circleSprite.texture);
			Destroy(circleSprite);
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "LevelComplete")
		{
			HidePlayer();
			return;
		}

		ApplyCustomization();

		if (IsLevelScene(scene))
		{
			BeginRespawn();
		}
	}

	private void HidePlayer()
	{
		isRespawning = true;
		movementInput = Vector2.zero;
		body.linearVelocity = Vector2.zero;
		body.simulated = false;

		foreach (SpriteRenderer spriteRenderer in spriteRenderers)
		{
			spriteRenderer.enabled = false;
		}

		foreach (Collider2D playerCollider in colliders)
		{
			playerCollider.enabled = false;
		}
	}

	public static void RefreshCustomization()
	{
		if (instance != null)
		{
			instance.ApplyCustomization();
		}
	}

	private void ApplyCustomization()
	{
		Customization.ApplySavedCustomization(playerRenderer, squareSprite, circleSprite);
	}

	private static bool IsLevelScene(Scene scene)
	{
		return scene.name == "Level1" || scene.name == "Level2" || scene.name == "Level3";
	}

	private void BeginRespawn()
	{
		if (respawnRoutine == null)
		{
			respawnRoutine = StartCoroutine(RespawnAfterDelay());
		}
	}

	private IEnumerator RespawnAfterDelay()
	{
		isRespawning = true;
		movementInput = Vector2.zero;
		body.linearVelocity = Vector2.zero;
		body.simulated = false;

		foreach (SpriteRenderer spriteRenderer in spriteRenderers)
		{
			spriteRenderer.enabled = false;
		}

		foreach (Collider2D playerCollider in colliders)
		{
			playerCollider.enabled = false;
		}

		yield return new WaitForSecondsRealtime(respawnDelay);

		body.simulated = bodyWasSimulated;
		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			spriteRenderers[i].enabled = spriteRendererStates[i];
		}

		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = colliderStates[i];
		}

		movementInput = Vector2.zero;
		isRespawning = false;
		respawnRoutine = null;
	}

	private void Update()
	{
		movementInput = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		).normalized;
	}

	private void FixedUpdate()
	{
		if (isRespawning)
		{
			return;
		}

		body.linearVelocity = movementInput * moveSpeed;
	}
}
