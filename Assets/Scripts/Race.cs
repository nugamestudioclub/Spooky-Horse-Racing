using UnityEngine;

public enum RaceState {
	None,
	Waiting,
	Countdown,
	Racing,
	Finished,
}

public class Race : MonoBehaviour {
	public static readonly int PlayerCount = 4;

	private static Race instance;

	[SerializeField]
	private RaceState state;

	[Range(0.0f, float.MaxValue)]
	[SerializeField]
	private float delayTime;

	private float currentTime;

	[SerializeField]
	private Timer timer;

	private readonly PlayerInfo[] playerInfo = new PlayerInfo[PlayerCount];

	void Awake() {
		if( instance == null ) {
			instance = this;
			DontDestroyOnLoad(gameObject);
			Initialize();
		}
		else {
			Destroy(gameObject);
		}
	}

	void Update() {
		switch( state ) {
		case RaceState.Startup:
			currentTime = Mathf.Max(currentTime - Time.deltaTime, 0.0f);
			if( Mathf.Approximately(currentTime, 0.0f) ) {
				timer.Show(true);
				timer.Begin();
				state = RaceState.Countdown;
			}
			break;
		case RaceState.Countdown:
			if( timer.IsDone ) {
				timer.Show(false);
				timer.Reset();
				state = RaceState.Racing;
			}
			break;
		case RaceState.Racing:
			break;
		case RaceState.Finished:
			break;
		}
	}

	private void Initialize() {
		state = RaceState.Startup;
		currentTime = delayTime;
	}

	public static void Register(int playerId, PlayerInfo playerInfo) {
		instance.playerInfo[playerId] = playerInfo;
		Debug.Log($"{playerId}: {playerInfo.Name}");
	}
}