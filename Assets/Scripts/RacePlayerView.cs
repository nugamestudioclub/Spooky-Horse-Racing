using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RacePlayerView : MonoBehaviour {
	[SerializeField]
	private int id;

	private bool isEnabled;
	public bool IsEnabled {
		get => isEnabled;
		set {
			nameText.gameObject.SetActive(value);
			placeText.gameObject.SetActive(value);
			placeImage.gameObject.SetActive(value);
			timeText.gameObject.SetActive(value);
			timeImage.gameObject.SetActive(value);
			coinText.gameObject.SetActive(value);
			coinImage.gameObject.SetActive(value);
			hitText.gameObject.SetActive(value);
			hitImage.gameObject.SetActive(value);
			isEnabled = value;
		}
	}

	[SerializeField]
	private TMP_Text nameText;

	[SerializeField]
	private TMP_Text placeText;

	[SerializeField]
	private Image placeImage;

	[SerializeField]
	private TMP_Text timeText;

	[SerializeField]
	private Image timeImage;

	[SerializeField]
	private TMP_Text coinText;

	[SerializeField]
	private Image coinImage;

	[SerializeField]
	private TMP_Text hitText;

	[SerializeField]
	private Image hitImage;

	void Start() {
		if( IsEnabled ) {
			var profile = Race.GetPlayerProfile(id);

			nameText.text = profile.Name;
		}
	}

	void Update() {
		if( IsEnabled ) {
			var stats = Race.GetPlayerStats(id);

			placeText.text = $"{ToOrdinal(stats.place)} / {Race.TotalRacers}";
			timeText.text = TimeSpan.FromSeconds(stats.time).ToString("m\\:ss\\.fff");
			coinText.text = stats.coinCount.ToString();
			hitText.text = stats.hitCount.ToString();
		}
	}

	private static string ToOrdinal(int value) {
		int magnitude = Math.Abs(value);
		int remainder = magnitude % 100;

		return (value < 0 ? "-" : "")
			+ magnitude
			+ (remainder >= 11 && remainder <= 13
				? "th"
				: (magnitude % 10) switch { 1 => "st", 2 => "nd", 3 => "rd", _ => "th" }
		);
	}
}