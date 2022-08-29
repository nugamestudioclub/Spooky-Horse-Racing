using System;
using UnityEngine;

public class RollPhysics : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D rb;

	[SerializeField]
	private float speed = 10f;

	private bool isGrounded = false;
	public bool IsGrounded { get { return isGrounded; } }

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}
	void Update() {
		/*
		if( InputController.GetStartDown(0) )
			Debug.Log($"start down at {Time.frameCount}");
		if( InputController.GetStartUp(0) )
			Debug.Log($"start up at {Time.frameCount}");
		if( InputController.GetCancelDown(0) )
			Debug.Log($"cancel down at {Time.frameCount}");
		if( InputController.GetCancelUp(0) )
			Debug.Log($"cancel up at {Time.frameCount}");
		if( InputController.GetJumpDown(0) )
			Debug.Log($"jump down at {Time.frameCount}");
		if( InputController.GetJumpUp(0) )
			Debug.Log($"jump up at {Time.frameCount}");
		if( InputController.GetFireDown(0) )
			Debug.Log($"fire down at {Time.frameCount}");
		if( InputController.GetFireUp(0) )
			Debug.Log($"fire up at {Time.frameCount}");
		*/
		//print(-InputController.GetMovement(0).x);
		rb.AddTorque(-InputController.GetMovement(0).x * Time.deltaTime * speed, ForceMode2D.Impulse);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
			this.isGrounded = true;
			print("Hit ground");
		}
		
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Ground"))
		{
			this.isGrounded = false;
		}
	}
}