using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PeopleScript : MonoBehaviour
{
	[SerializeField, Min(0f)] private float minimumSpeed = 1f;
	[SerializeField, Min(0f)] private float maximumSpeed = 3f;

	private Rigidbody2D body;
	private SpriteRenderer spriteRenderer;
	private float currentSpeed;
	private int zombieHits;

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		body.gravityScale = 0f;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		currentSpeed = Random.Range(minimumSpeed, maximumSpeed);
		Vector2 direction = Random.insideUnitCircle.normalized;
		if (direction == Vector2.zero)
		{
			direction = Vector2.right;
		}

		body.linearVelocity = direction * currentSpeed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (TryBecomeZombie(collision.gameObject))
		{
			return;
		}

		if (collision.contactCount == 0)
		{
			return;
		}

		Vector2 normal = collision.GetContact(0).normal;
		Vector2 directionAwayFromWall = Vector2.Reflect(body.linearVelocity.normalized, normal);
		currentSpeed = Random.Range(minimumSpeed, maximumSpeed);
		body.linearVelocity = directionAwayFromWall.normalized * currentSpeed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		TryBecomeZombie(other.gameObject);
	}

	private bool TryBecomeZombie(GameObject touchingObject)
	{
		ZombieScript zombie = touchingObject.GetComponentInParent<ZombieScript>();
		PlayerController player = touchingObject.GetComponentInParent<PlayerController>();
		if (zombie == null && player == null)
		{
			return false;
		}

		if (player == null && ++zombieHits < 2)
		{
			return false;
		}

		SpriteRenderer sourceRenderer = zombie != null
			? zombie.GetComponentInChildren<SpriteRenderer>()
			: player.GetComponentInChildren<SpriteRenderer>();
		if (spriteRenderer != null && sourceRenderer != null)
		{
			spriteRenderer.sprite = sourceRenderer.sprite;
			spriteRenderer.color = sourceRenderer.color;
			spriteRenderer.flipX = sourceRenderer.flipX;
			spriteRenderer.flipY = sourceRenderer.flipY;
		}

		body.linearVelocity = Vector2.zero;
		enabled = false;
		gameObject.AddComponent<ZombieScript>();
		Destroy(this);
		return true;
	}
}
