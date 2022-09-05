using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyPlayerView : MonoBehaviour {
	[SerializeField]
	private int playerId;

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
			if( value ) {
				connectText.enabled = false;
				nameInputField.gameObject.SetActive(true);
				readyText.enabled = true;
				readySpriteRenderer.enabled = true;
			}
			else {
				connectText.enabled = true;
				nameInputField.gameObject.SetActive(false);
				readyText.enabled = false;
				readySpriteRenderer.enabled = false;
			}
		}
	}

	void Start() {
		IsConnected = false;
		IsReady = false;
	}
}