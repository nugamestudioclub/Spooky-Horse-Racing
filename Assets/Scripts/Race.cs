using UnityEngine;

public enum RaceState {
	None,
	Waiting,
	Countdown,
	Racing,
	Finished,
}

public class Race : MonoBehaviour {
	public static readonly int MaxPlayers = 4;

	public static readonly int MaxRacers = 8;

	private int activePlayerCount;

	public static readonly int GhostCount = 4;

	private int ActiveRacerCount => activePlayerCount + GhostCount;

	private static Race instance;

	[SerializeField]
	private RaceState state;

	[Range(0.0f, float.MaxValue)]
	[SerializeField]
	private float delayTime;

	private float currentTime;

	[SerializeField]
	private Timer timer;

	private static readonly PlayerInfo[] playerInfo = new PlayerInfo[MaxRacers];

	[SerializeField]
	private Transform[] spawnPoints = new Transform[MaxRacers];

	[SerializeField]
	private GameObject[] playerPrefabs = new GameObject[MaxPlayers];

	[SerializeField]
	private GameObject[] ghostPrefabs = new GameObject[GhostCount];

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

	void Start() {
		SpawnRacers();
	}

	void Update() {
		switch( state ) {
		case RaceState.Waiting:
			Tick();
			if( DoneWaiting() ) {
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
			Clear();
			break;
		}
	}

	private void Initialize() {
		currentTime = delayTime;
		state = RaceState.Waiting;
	}

	public static void Register(int playerId, PlayerInfo playerRegistration) {
		playerInfo[playerId] = playerRegistration;
	}

	private void Tick() {
		currentTime = Mathf.Max(currentTime - Time.deltaTime, 0.0f);
	}

	private bool DoneWaiting() {
		return Mathf.Approximately(currentTime, 0.0f);
	}

	private void SpawnRacers() {
		int pos = 0;

		for( int i = 0; i < MaxPlayers; ++i )
			if( playerInfo[i] != null )
				Spawn(playerPrefabs[i], spawnPoints[pos++]);
		for( int i = 0; i < GhostCount; ++i )
			Spawn(ghostPrefabs[i], spawnPoints[pos++]);
	}

	private void Spawn(GameObject obj, Transform transform) {
		Instantiate(obj, transform.position, transform.rotation);
	}

	private void Clear() {
		for( int i = 0; i < MaxPlayers; ++i )
			playerInfo[i] = null;
	}
}