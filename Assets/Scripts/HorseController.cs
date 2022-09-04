using System.Collections.Generic;
using UnityEngine;


public enum RecordingState { Record, Run };
public class HorseController : MonoBehaviour {
	[SerializeField]
	private RollPhysics target;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private Vector2 prevPos;

	private List<Vector2> positions;
	private List<float> rotation;
	public RecordingState recordingState;
	private int curIndex = 0;
	private int id = 0;
	private bool callbackMade = false;

	private int loadCurIndex = 0;
	private Vector2 curTarget;

	void Start() {

		prevPos = target.transform.position;
		id = PlayerPrefs.GetInt("LastestId");
		if( id == 0 ) {
			PlayerPrefs.SetInt("LatestId", Random.Range(10000, 01000000));
		}
		PlayerPrefs.SetInt("LatestId", id + 1);
		positions = new List<Vector2>();
		rotation = new List<float>();
		curTarget = transform.position;
	}

	void Update() {

		transform.position = target.transform.position;

		if( recordingState == RecordingState.Run && Vector2.Distance(transform.position, positions[curIndex]) < 0.3f ) {
			curIndex = (curIndex + 1) % positions.Count;
		}
		if( recordingState == RecordingState.Run ) {
			transform.position = Vector2.Lerp((Vector2)transform.position, curTarget, 0.5f);
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation[curIndex]);
		}
	}

	private void FixedUpdate() {
		HandleRotation();
		HandlePosition();

		if( recordingState == RecordingState.Record ) {
			SaveTransform();
		}
		else {
			LoadTransform();
		}
	}

	void SaveTransform() {
		int lastInput = PlayerPrefs.GetInt("LastInput" + id);

		PlayerPrefs.SetString("VectorPos" + lastInput, JsonUtility.ToJson(new PlayerSaveData(transform.position, transform.rotation.x)));
		PlayerPrefs.SetInt("LastInput" + id, lastInput + 1);

	}
	void LoadTransform() {

		string data = PlayerPrefs.GetString("VectorPos" + loadCurIndex);
		PlayerSaveData sData = JsonUtility.FromJson<PlayerSaveData>(data);
		curTarget = transform.position;
		transform.eulerAngles = new Vector2(sData.rotation, 0);


		loadCurIndex++;
	}

	private void HandleRotation() {
		if( target.IsGrounded ) {
			float angle = 360 * Mathf.Atan2(target.Orientation.y, target.Orientation.x) / (2 * Mathf.PI);
			bool flip = InputController.GetMovement(0).x < 0;

			spriteRenderer.flipX = flip;
			angle -= 90;

			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
				transform.localEulerAngles.y,
				angle);
		}

		else {
			Vector2 delta = (Vector2)target.transform.position - prevPos;

			float angle = 360 * Mathf.Atan2(delta.y, delta.x) / (2 * Mathf.PI);
			bool flip = Mathf.Abs(angle) > 90;

			spriteRenderer.flipX = flip;
			angle -= flip ? 180 : 0;

			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
				transform.localEulerAngles.y,
				angle);
		}
	}


	/*
	private void HandleRotation() {
		bool flip = InputController.GetMovement(0).x < 0;
		float angle = 360 / (2 * Mathf.PI);

		if( target.IsGrounded ) {
			angle *= Mathf.Atan2(target.Orientation.y, target.Orientation.x);
			angle += 90;
		}
		else {
			var delta = (Vector2)target.transform.position - prevPos;
			angle *= Mathf.Atan2(delta.y, target.Orientation.x);
			angle -= flip ? 180 : 0;
		}

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
			transform.localEulerAngles.y,
			angle);
	}
	*/



	private void HandlePosition() {
		prevPos = target.transform.position;

		if( recordingState == RecordingState.Record ) {
			positions.Add(transform.position);
			rotation.Add(transform.eulerAngles.z);
		}
	}
}

/// <summary>
/// Structure representation of ghost runs.
/// </summary>
[System.Serializable]
public class SaveData {
	[SerializeField]
	public PlayerSaveData[] data;


}
/// <summary>
/// Structure representation of one ghost run.
/// </summary>
[System.Serializable]
public class PlayerSaveData {
	public Vector2 position;
	public float rotation;

	public PlayerSaveData(Vector2 pos, float rot) {
		position = pos;
		rotation = rot;
	}
}

