using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyPlayerView : MonoBehaviour {
	[SerializeField]
	private int playerId;

	[SerializeField]
	private SpriteSheet[] knightSpriteSheets;

	[SerializeField]
	private Image knightImage;

	[SerializeField]
	private SpriteSheet[] horseSpriteSheets;

	[SerializeField]
	private Image horseImage;

	[SerializeField]
	private TMP_InputField nameInputField;

	[SerializeField]
	private TMP_Text readyText;

	[SerializeField]
	private SpriteRenderer readySpriteRenderer;

	[SerializeField]
	private TMP_Text connectText;

	public string Name {
		get => string.IsNullOrEmpty(nameInputField.text) ? (nameInputField.placeholder as TMP_Text).text : nameInputField.text;
		set {
			if( string.IsNullOrEmpty(nameInputField.text) )
				(nameInputField.placeholder as TMP_Text).text = value;
			else
				nameInputField.text = value;
		}
	}

	private int knightIndex;
	public int KnightIndex {
		get => knightIndex;
		set {
			knightIndex = (value + knightSpriteSheets.Length) % knightSpriteSheets.Length;
			knightImage.sprite = Knight[0];
		}
	}
	public SpriteSheet Knight => knightSpriteSheets[knightIndex];

	private int horseIndex;
	public int HorseIndex {
		get => horseIndex;
		set {
			horseIndex = (value + horseSpriteSheets.Length) % horseSpriteSheets.Length;
			horseImage.sprite = Horse[0];
		}
	}

	public SpriteSheet Horse => horseSpriteSheets[horseIndex];

	private bool isReady;
	public bool IsReady {
		get => isReady;
		set {
			Color color;
			string text;

			if( value ) {
				color = Color.green;
				text = "Ready";
			}
			else {
				color = Color.red;
				text = "Not\nReady";
			}

			readySpriteRenderer.color = color;
			readyText.text = text;
			isReady = value;
		}
	}

	private bool isConnected;
	public bool IsConnected {
		get => isConnected;
		set {
			isConnected = value;
			connectText.enabled = !value;
			nameInputField.gameObject.SetActive(value);
			readyText.enabled = value;
			readySpriteRenderer.enabled = value;
			knightImage.gameObject.SetActive(value);
			horseImage.gameObject.SetActive(value);
		}
	}

	void Start() {
		IsConnected = false;
		IsReady = false;
		KnightIndex = 0;
		HorseIndex = 0;
	}
}