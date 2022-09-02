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
		if( value < 0 )
			return value.ToString();

		int remainder = value % 100;

		if( remainder >= 11 && remainder <= 13 )
			return value + "th";

		return (value % 10) switch {
			1 => value + "st",
			2 => value + "nd",
			3 => value + "rd",
			_ => value + "th",
		};
	}
}