using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowScript : MonoBehaviour {
	private Animator animator;
	[SerializeField]
	private GameObject arrowPrefab;
	[SerializeField]
	private float bowPower = 5;

	void Awake() {
		animator = GetComponent<Animator>();
	}

	void Start() {
		animator.Play("idle");
	}

	void Update() {
		if( InputController.GetFireDown(0) ) ///
			ChargeBow();

		if( InputController.GetFireUp(0) ) ///
			FireBow();
	}

	private void ChargeBow() {
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("idle") ) {
			animator.Play("bowCharge");
		}
	}

	private void FireBow() {
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("bowCharge") ) {
			var direction = CalcDirection();
			float charge = CalcCharge();
			GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.right * 2, transform.rotation);
			Rigidbody2D rigidbody2D = arrow.GetComponent<Rigidbody2D>();

			rigidbody2D.AddForce(direction * charge * bowPower, ForceMode2D.Impulse);
			animator.Play("fireReload");
		}
	}

	private Vector3 CalcDirection() {
		var aim = InputController.GetAim(0); ///
		float angle = Mathf.Atan2(aim.y, aim.x) * 180 / Mathf.PI;

		return Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
	}

	private float CalcCharge() {
		return Mathf.Max(
			Mathf.Round(animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 10 / 2) + 1,
			0.0f
		);
	}
}