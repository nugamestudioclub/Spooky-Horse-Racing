using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
	private Transform rightArmBone;
	private Transform leftBicepBone;
	private Transform leftArmBone;
	private Transform rightArm;
	private Transform bow;
	private float angle;
	private bool reloading;

	// Start is called before the first frame update
	void Start() {
		rightArmBone = transform.Find("rightArmBone");
		leftBicepBone = transform.Find("leftBicepBone");
		leftArmBone = leftBicepBone.Find("leftArmBone");
		rightArm = transform.Find("rightArm");
		bow = rightArm.Find("bow");
		reloading = false;
	}

	// Update is called once per frame
	void Update() {
		var movement = InputController.GetMovement(0); ///
		angle = Mathf.Atan2(movement.y, movement.x) * 180 / Mathf.PI;

		rightArmBone.rotation = Quaternion.Euler(0, 0, angle);
		leftBicepBone.rotation = Quaternion.Euler(0, 0, angle);
		bow.rotation = Quaternion.Euler(0, 0, angle);

		if( (angle > 90 && angle < 270) || (angle < -90 && angle > -270) ) {
			transform.rotation = Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z);
		}
		else {
			transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
		}

		Transform thighBone = transform.Find("torsoBone").Find("thighBone");
		thighBone.rotation = Quaternion.Euler(0, 0, thighBone.rotation.eulerAngles.z);

		if( !reloading ) {
			if( InputController.GetFireDown(0) ) { ///
				StartCoroutine(PullBack());
			}
		}

		if( InputController.GetFireUp(0) ) { ///
			StopAllCoroutines();
			reloading = true;
			leftArmBone.localPosition = new Vector3(0.5850446f, 1.882562e-07f, 0);
			StartCoroutine(WaitToReload());
		}

		// Following code is for demo purposes
		var demoMovement = InputController.GetMovement(0); /// 
		if( !Mathf.Approximately(demoMovement.magnitude, 0.0f) )
			transform.Translate(50 * Time.deltaTime * demoMovement, Space.World);
		// end demo code
	}

	private IEnumerator WaitToReload() {
		while( bow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("fireReload") ) {
			yield return null;
		}

		reloading = false;
	}


	private IEnumerator PullBack() {
		yield return new WaitForSeconds(0.2f);

		while( leftArmBone.transform.localPosition.x > 0.2 ) {
			leftArmBone.Translate(-.19f, 0, 0);

			yield return new WaitForSeconds(0.2f);
		}
	}
}
