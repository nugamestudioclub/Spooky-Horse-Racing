using System;
using UnityEngine;

public struct PlayerStats {
	public int place;
	public float time;
	public int coinCount;
	public int hitCount;
	public bool isGhost;
}

public class RacePlayer : MonoBehaviour {
	[SerializeField]
	private TMPro.TMP_Text nameplateText;

	public string Name {
		get => nameplateText.text;
		set => nameplateText.text = value;
	}

	private int id;
	public int Id {
		get => id;
		set {
			nameplateText.gameObject.layer = LayerMask.NameToLayer("Not_Player_" + value);
			id = value;
		}
	}

	[SerializeField]
	private RacePlayerMovement racePlayerMovement;

	[SerializeField]
	private RacePlayerMovement recording;

	[SerializeField]
	private AnimationManager horseAnimationManager;

	public Transform Transform => racePlayerMovement == null ? transform : racePlayerMovement.transform;

	public SpriteSheet Horse {
		get => horseAnimationManager.SpriteSheet;
		set => horseAnimationManager.SpriteSheet = value;
	}

	public bool ControlEnabled {
		get => racePlayerMovement == null ? false : racePlayerMovement.ControlEnabled;
		set {
			if( racePlayerMovement != null )
				racePlayerMovement.ControlEnabled = value;
		}
	}

	public bool HasReachedMidpoint { get; set; }

	public bool HasReachedFinishLine { get; set; }

	public int Place { get; set; }

	private float time;
	public float Time {
		get => time;
		set => time = Mathf.Max(value, 0.0f);
	}

	private int coinCount;
	public int CoinCount {
		get => coinCount;
		set => coinCount = Math.Max(value, 0);
	}

	private int hitCount;
	public int HitCount {
		get => hitCount;
		set => hitCount = Math.Max(value, 0);
	}

	private bool isGhost;
	public bool IsGhost {
		get => isGhost;
		set => isGhost = value;
	}

    public PlayerStats Stats() {
		return new PlayerStats {
			place = Place,
			time = time,
			coinCount = coinCount,
			hitCount = hitCount,
			isGhost = isGhost
		};
	}

	public void SetController(int id) {
		var racePlayerMovement = GetComponentsInChildren<RacePlayerMovement>();

		if( racePlayerMovement.Length > 0 )
			racePlayerMovement[0].ControllerId = id;
	}

	public void BeginRecording() {
		recording.ControlEnabled = true;
		recording.BeginRecording();
	}

	public void EndRecording() {
		recording.ControlEnabled = false;
		recording.EndRecording();
	}
}