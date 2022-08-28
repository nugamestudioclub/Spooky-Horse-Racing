using System;
using UnityEngine;

public class RollPhysics : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D rb;

	[SerializeField]
	private float speed = 10f;

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

		rb.AddTorque(-InputController.GetMovement(0).x * Time.deltaTime * speed, ForceMode2D.Impulse);
	}
}