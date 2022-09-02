using System;
using UnityEngine;

public struct PlayerStats {
	public int place;
	public float time;
	public int coinCount;
	public int hitCount;
}

public class RacePlayer : MonoBehaviour {
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

	public PlayerStats Stats() {
		return new PlayerStats {
			place = place,
			time = time,
			coinCount = coinCount,
			hitCount = hitCount
		};
	}
}