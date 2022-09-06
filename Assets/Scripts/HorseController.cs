using System.Collections.Generic;
using UnityEngine;


public enum RecordingState { Record, Run };
public class HorseController : MonoBehaviour {
	[SerializeField]
	private RacePlayerMovement target;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	Animator animator;

	private Vector2 prevPos;

	private List<Vector2> positions;
	private List<float> rotation;
	public RecordingState recordingState;
	private int curIndex = 0;
	private int id = 0;
	private bool callbackMade = false;

	private int loadCurIndex = 0;
	private Vector2 curTarget;

	private Vector2 lastMovement;

	private bool isFalling;

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

		
	}

	private void FixedUpdate() {
		HandleRotation();
		HandleAnimations();
		HandlePosition();
		

		if( recordingState == RecordingState.Record ) {
			//SaveTransform();
		}
		else {
			//LoadTransform();
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
		var (angle, flipped) = CalcRotationAndFlip();

		spriteRenderer.flipX = flipped;
		transform.localEulerAngles = new Vector3(
			transform.localEulerAngles.x,
			transform.localEulerAngles.y,
			angle);
	}

	private (float, bool) CalcRotationAndFlip() {
		float angle = 360 / (2 * Mathf.PI);
		bool flipped;

		if( target.ControlEnabled && target.IsGrounded ) {
			angle = (angle * Mathf.Atan2(target.Orientation.y, target.Orientation.x)) - 90;
			Vector2 movement = InputController.GetMovement(target.ControllerId);
			if( Mathf.Approximately(movement.x, 0.0f) ) {
				flipped = lastMovement.x < 0;
			}
			else {
				flipped = movement.x < 0;
				lastMovement = movement;
			}
		}
		else {
			Vector2 delta = (Vector2)target.transform.position - prevPos;
			angle = (angle * Mathf.Atan2(delta.y, delta.x));
			flipped = Mathf.Abs(angle) > 90;
			angle += flipped ? 180 : 0;
		}

		return (angle, flipped);
	}


	private void HandlePosition() {
		prevPos = target.transform.position;

		if( recordingState == RecordingState.Record ) {
			positions.Add(transform.position);
			rotation.Add(transform.eulerAngles.z);
		}
	}

	private void HandleAnimations() {
		if( target.ControlEnabled && target.IsGrounded ) {
			if( InputController.GetJumpUp(target.ControllerId) ) {
				animator.Play("horse_jump");
			}
			else if( isFalling ) {
				animator.Play("horse_land");
				isFalling = false;
			}
			else {
				//change animation speed based on speed
				//animator.speed = target.Speed / target.MaxSpeed;
			}
		}
		else if( target.Velocity.y < 0 ) {
			animator.Play("horse_fall");
			isFalling = true;

		}
		else {
			animator.speed = 1;
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

