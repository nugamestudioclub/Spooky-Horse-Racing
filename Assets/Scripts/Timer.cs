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
	
	[SerializeField]
	private float currentTime;

	public bool IsRunning { get; private set; }

	public bool IsDone => Mathf.Approximately(currentTime, 0.0f);

	void Start() {
		Reset();
	}

	void FixedUpdate() {
		if( IsRunning )
			Tick(Time.fixedDeltaTime);
	}

	public void Begin() {
		IsRunning = true;
		Draw();
		Show(true);
	}

	private void Tick(float delta) {
		currentTime = Mathf.Max(currentTime - delta, 0.0f);
		Draw();
	}

	public void Reset() {
		IsRunning = false;
		currentTime = startTime;
		Show(false);
	}

	public void Show(bool value) {
		text.gameObject.SetActive(value);
		clockImage.gameObject.SetActive(value);
	}

	private void Draw() {
		clockImage.fillAmount = 1.0f - (currentTime / startTime);
		text.text = ((int)Mathf.Ceil(currentTime)).ToString();
	}
}