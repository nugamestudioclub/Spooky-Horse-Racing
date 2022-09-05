using System;
using UnityEngine;

public class RollPhysics : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D rb;

	[SerializeField]
	CircleCollider2D circleCollider;

	[SerializeField]
	private float speed = 100;

	[SerializeField]
	private float jumpStrength = 60;

	public Vector2 Orientation { get; private set; }

	public bool IsGrounded { get; private set; }

	[SerializeField]
	private float jumpMinimum = 0.3f;

	[SerializeField]
	private float jumpAcceleration = 1.0f;

	private float jumpScale;

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
		if( IsGrounded ) {
			if( InputController.GetJump(0) ) {
				jumpScale = Mathf.Clamp(jumpScale + jumpAcceleration * Time.deltaTime, jumpMinimum, 1.0f);
			}
			else if( InputController.GetJumpUp(0) ) {
				Debug.Log(jumpScale);
				rb.AddForce(jumpScale * jumpStrength * Orientation, ForceMode2D.Impulse);
			}
		}
		else {
			jumpScale = 0.0f;
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