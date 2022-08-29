using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {
	[SerializeField]
	private int playerId;

	[SerializeField]
	private TMP_InputField nameInputField;

	[SerializeField]
	private TMP_Text readyText;

	[SerializeField]
	private SpriteRenderer readySpriteRenderer;

	public string Name {
		get => nameInputField.text;
		set => (nameInputField.placeholder as TMP_Text).text = value;
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
}