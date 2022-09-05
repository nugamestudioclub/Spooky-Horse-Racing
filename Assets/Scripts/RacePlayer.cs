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
	private RollPhysics physics;

	public bool ControlEnabled {
		get => physics == null ? false : physics.ControlEnabled;
		set {
			if( physics != null )
				physics.ControlEnabled = value;
		}
	}

	private int place;
	public int Place {
		get => place;
		set => place = Math.Min(Math.Max(1, value), Race.MaxRacers);
	}

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
		set => HitCount = Math.Max(value, 0);
	}

	private bool isGhost;
	public bool IsGhost {
		get => isGhost;
		set => isGhost = value;
	}

	public PlayerStats Stats() {
		return new PlayerStats {
			place = place,
			time = time,
			coinCount = coinCount,
			hitCount = hitCount,
			isGhost = isGhost
		};
	}

	public void SetController(int id) {
		var rollPhysics = GetComponentsInChildren<RollPhysics>();

		if( rollPhysics.Length > 0 )
			rollPhysics[0].ControllerId = id;
	}
}