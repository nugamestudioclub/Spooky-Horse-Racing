using TMPro;
using UnityEngine;

public class ResultsPlayerView : MonoBehaviour {
	[SerializeField]
	private TMP_Text placeText;

	[SerializeField]
	private TMP_Text nameText;

	[SerializeField]
	private TMP_Text timeText;

	[SerializeField]
	private TMP_Text hitsText;

	[SerializeField]
	private TMP_Text coinsText;

	private static readonly Color DefaultColor = Color.white;

	private static readonly Color BestColor = new Color(1.0f, 0.5f, 0.0f);

	private bool isVisible;
	public bool IsVisible {
		get => isVisible;
		set {
			placeText.gameObject.SetActive(value);
			nameText.gameObject.SetActive(value);
			timeText.gameObject.SetActive(value);
			hitsText.gameObject.SetActive(value);
			coinsText.gameObject.SetActive(value);
			isVisible = value;
		}
	}

	public void SetPlace(int value, bool isBest = false) {
		placeText.text = Strings.ToOrdinal(value);
		placeText.color = isBest ? BestColor : DefaultColor;
	}

	public void SetName(string name) {
		nameText.text = name;
	}

	public void SetTime(float seconds, bool isBest = false) {
		timeText.text = Strings.TimestampFromSeconds(seconds);
		coinsText.color = isBest ? BestColor : DefaultColor;
	}

	public void SetHits(int value, bool isBest = false) {
		hitsText.text = value.ToString();
		hitsText.color = isBest ? BestColor : DefaultColor;
	}

	public void SetCoins(int value, bool isBest = false) {
		coinsText.text = value.ToString();
		coinsText.color = isBest ? BestColor : DefaultColor;
	}

	public void Draw(PlayerResults stats) {
		SetName(stats.name);
		SetPlace(stats.place, stats.isPlaceBest);
		SetTime(stats.time, stats.isTimeBest);
		SetHits(stats.hitCount, stats.isHitsBest);
		SetCoins(stats.coinCount, stats.isCoinsBest);
	}
}