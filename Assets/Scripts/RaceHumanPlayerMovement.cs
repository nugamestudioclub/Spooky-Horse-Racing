using System;
using System.Collections;
using UnityEngine;

public class RaceHumanPlayerMovement : RacePlayerMovement
{
	

	

	[SerializeField]
	private Rigidbody2D rb;

	[SerializeField]
	CircleCollider2D circleCollider;	

	[SerializeField]
	private float jumpStrength = 60;

	[SerializeField]
	private float jumpMinimum = 0.3f;

	[SerializeField]
	private float jumpAcceleration = 1.0f;

	private float jumpScale;

	[SerializeField]
	private float acceleration = 100;

	private float speed = 100;
	public float Speed { get => speed; private set => speed = value; }

	[SerializeField]
	private float maxSpeed = 100;
	public float MaxSpeed { get => maxSpeed; private set => maxSpeed = value; }

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}
	void Update() {
		HandleOrientation();
		if( ControlEnabled ) {
			HandleMovement();
			HandleJump();
		}
	}
	private void HandleJump() {
		if( IsGrounded ) {
			if( InputController.GetJump(ControllerId) ) {
				jumpScale = Mathf.Clamp(jumpScale + jumpAcceleration * Time.deltaTime, jumpMinimum, 1.0f);
			}
			else if( InputController.GetJumpUp(ControllerId) ) {
				rb.AddForce(jumpScale * jumpStrength * Orientation, ForceMode2D.Impulse);
			}
		}
		else {
			jumpScale = 0.0f;
		}
	}

	private void HandleMovement() {
		rb.AddTorque(-InputController.GetMovement(ControllerId).x * Time.deltaTime * acceleration, ForceMode2D.Impulse);
		Speed = Mathf.Clamp(rb.velocity.magnitude, 0f, maxSpeed);
		if (rb.velocity.magnitude > maxSpeed)
        {
			rb.velocity = rb.velocity.normalized * maxSpeed;
        }
		Velocity = rb.velocity;
	}

	private void HandleOrientation() {
		if( IsGrounded ) {
			const int CAPACITY = 3;
			var contactPoints = new ContactPoint2D[CAPACITY];
			int size = circleCollider.GetContacts(contactPoints);

			Orientation = contactPoints[0].normal;
		}
	}
	private void OnCollisionEnter2D(Collision2D collision) {
		if( collision.gameObject.CompareTag("Ground") ) {
			IsGrounded = true;
		}

	}
	private void OnCollisionExit2D(Collision2D collision) {
		if( collision.gameObject.CompareTag("Ground") ) {
			IsGrounded = false;
		}
	}

	public override void Play() { }

	public override void Stop() { }

	public override void Freeze()
    {
		rb.velocity = Vector2.zero;
		rb.gravityScale = 0;
		Velocity = Vector2.zero;
		Speed = 0;
		ControlEnabled = false;
    }

	public override void Freeze(float duration)
    {
		StartCoroutine(FreezeFor(duration));
    }

	public override void UnFreeze()
    {
		rb.gravityScale = 5;
		ControlEnabled = true;
    }

	private IEnumerator FreezeFor(float duration)
    {
		Freeze();

		yield return new WaitForSeconds(duration);

		UnFreeze();
    }
}