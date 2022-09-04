using System;
using UnityEngine;

public class RollPhysics : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D rb;

	[SerializeField]
	CircleCollider2D circleCollider;

	[SerializeField]
	private float speed = 100f;

	[SerializeField]
	private float jumpStrength = 50;

	public Vector2 Orientation { get; private set; }

	public bool IsGrounded { get; private set; }

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		HandleOrientation();
		HandleMovement();
		HandleJump();
	}

	private void HandleMovement() {
		rb.AddTorque(-InputController.GetMovement(0).x * Time.deltaTime * speed, ForceMode2D.Impulse);
	}

	private void HandleOrientation() {
		if( IsGrounded ) {
			const int CAPACITY = 3;
			var contactPoints = new ContactPoint2D[CAPACITY];
			int size = circleCollider.GetContacts(contactPoints);
			
			Orientation = contactPoints[0].normal;
		}
	}

	private void HandleJump() {
		if( InputController.GetJumpDown(0) && IsGrounded ) {
			rb.AddForce(jumpStrength * Orientation, ForceMode2D.Impulse);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if( collision.gameObject.CompareTag("Ground") ) {
			IsGrounded = true;
			print("Hit ground");
		}

	}
	private void OnCollisionExit2D(Collision2D collision) {
		if( collision.gameObject.CompareTag("Ground") ) {
			IsGrounded = false;
		}
	}
}