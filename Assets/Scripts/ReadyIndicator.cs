using UnityEngine;
using TMPro;

public class ReadyIndicator : MonoBehaviour {
	[SerializeField]
	private TMP_Text textMesh;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private int playerId;

	void Start() {
		Ready(false);
	}

	void Update() {
		if( InputController.GetStartDown(playerId) )
			Ready(true);
		else if( InputController.GetCancelDown(playerId) )
			Ready(false);
	}

	private void Ready(bool isReady) {
		Color color;
		string text;

		if( isReady ) {
			color = Color.green;
			text = "Ready";
		}
		else {
			color = Color.red;
			text = "Not\nReady";
		}

		spriteRenderer.color = color;
		textMesh.text = text;
	}
}