using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	[SerializeField]
	private Transform target;

	public Transform Target {
		get => target;
		set => target = value;
	}


	[SerializeField]
	private Camera myCamera;

	public Camera Camera => myCamera;

	[SerializeField]
	private float followStrength;

	void LateUpdate() {
		if( target != null )
			myCamera.transform.position = target.position + Vector3.back;
	}
}