using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	[SerializeField]
	private TMP_Text text;

	[SerializeField]
	private Image clockImage;

	[Range(1.0f, float.MaxValue)]
	[SerializeField]
	private float startTime;

	public float currentTime;

	public bool IsRunning { get; private set; }

	public bool IsDone => Mathf.Approximately(currentTime, 0.0f);

	void Start() {
		Reset();
	}

	public void Begin() {
		IsRunning = true;
		Draw();
		Show(true);
	}

	public void Tick() {
		currentTime = Mathf.Max(currentTime - Time.deltaTime, 0f);
		Draw();
	}

	public void Reset() {
		IsRunning = false;
		currentTime = startTime;
		Show(false);
	}

	private void Draw() {
		clockImage.fillAmount = currentTime / startTime;
		text.text = ((int)Mathf.Ceil(currentTime)).ToString();
	}

	private void Show(bool value) {
		clockImage.gameObject.SetActive(value);
	}
}