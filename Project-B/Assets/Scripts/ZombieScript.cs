using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieScript : MonoBehaviour
{
	[SerializeField, Min(0f)] private float minimumSpeed = 1f;
	[SerializeField, Min(0f)] private float maximumSpeed = 3f;

	private Rigidbody2D body;
	private float currentSpeed;

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		body.gravityScale = 0f;
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
		if (collision.contactCount == 0)
		{
			return;
		}

		Vector2 normal = collision.GetContact(0).normal;
		Vector2 directionAwayFromWall = Vector2.Reflect(body.linearVelocity.normalized, normal);
		currentSpeed = Random.Range(minimumSpeed, maximumSpeed);
		body.linearVelocity = directionAwayFromWall.normalized * currentSpeed;
	}
}
