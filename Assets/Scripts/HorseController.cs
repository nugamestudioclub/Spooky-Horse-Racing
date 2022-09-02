using System.Collections.Generic;
using UnityEngine;


public enum RecordingState { Record, Run };
public class HorseController : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D target;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private Vector2 prevPos;

	private List<Vector2> positions;
	private List<float> rotation;
	public RecordingState recordingState;
	private int curIndex = 0;
	private int id = 0;
	private bool callbackMade = false;

	private int loadCurIndex=0;
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

		HandleJump();
	}

	void SaveTransform() {
		int lastInput = PlayerPrefs.GetInt("LastInput" + this.id);

		PlayerPrefs.SetString("VectorPos" + lastInput, JsonUtility.ToJson(new PlayerSaveData(this.transform.position, this.transform.rotation.x)));
		PlayerPrefs.SetInt("LastInput" + this.id, lastInput + 1);

	}
	void LoadTransform() {

		string data = PlayerPrefs.GetString("VectorPos" + loadCurIndex);
		PlayerSaveData sData = JsonUtility.FromJson<PlayerSaveData>(data);
		this.curTarget = transform.position;
		this.transform.eulerAngles = new Vector2(sData.rotation, 0);


		loadCurIndex++;
	}

	private void HandleRotation() {
		Vector2 delta = (Vector2)target.transform.position - prevPos;
		float angle = 360 * Mathf.Atan2(delta.y, delta.x) / (2 * Mathf.PI);
		bool flip = Mathf.Abs(angle) > 90;

		spriteRenderer.flipX = flip;
		angle -= flip ? 180 : 0;

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
			transform.localEulerAngles.y,
			angle);
	}

	private void HandlePosition() {
		prevPos = target.transform.position;

		if( recordingState == RecordingState.Record ) {
			positions.Add(transform.position);
			rotation.Add(transform.eulerAngles.z);
		}
	}

	private void HandleJump() {
		if( InputController.GetJumpUp(0) && target.GetComponent<RollPhysics>().IsGrounded ) {
			target.AddForce((Vector2)transform.up * 25, ForceMode2D.Impulse);
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
		this.position = pos;
		this.rotation = rot;
	}
}

