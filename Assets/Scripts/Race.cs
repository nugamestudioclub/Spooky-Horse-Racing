using System;
using System.Linq;
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

	public int ActivePlayers => playerProfiles.Count(x => x != null);

	public static readonly int GhostCount = 4;

	public static int TotalRacers => instance.ActivePlayers + GhostCount;

	private static Race instance;

	[SerializeField]
	private CameraFollow[] cameras = new CameraFollow[MaxPlayers];

	[SerializeField]
	private RaceState state;

	[Range(0.0f, float.MaxValue)]
	[SerializeField]
	private float delayTime;

	private float currentTime;

	[SerializeField]
	private Timer timer;

	private static readonly PlayerProfile[] playerProfiles = new PlayerProfile[MaxRacers];

	[SerializeField]
	private Transform[] spawnPoints = new Transform[MaxRacers];

	[SerializeField]
	private GameObject[] humanPrefabs = new GameObject[MaxPlayers];

	[SerializeField]
	private GameObject[] ghostPrefabs = new GameObject[GhostCount];

	private RacePlayer[] humanRacers = new RacePlayer[MaxPlayers];

	private RacePlayer[] ghostRacers = new RacePlayer[GhostCount];

	[SerializeField]
	private RacePlayerView[] playerViews = new RacePlayerView[MaxPlayers];

	void Awake() {
		instance = this;
		Initialize();
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
		foreach( CameraFollow camera in cameras ) {
			camera.Camera.enabled = false;
		}
	}

	public static void Register(int playerId, PlayerProfile playerProfile) {
		playerProfiles[playerId] = playerProfile;
	}

	public static PlayerProfile GetPlayerProfile(int playerId) {
		return playerProfiles[playerId];
	}

	public static PlayerStats GetPlayerStats(int playerId) {
		return instance.humanRacers[playerId] == null
			? new PlayerStats()
			: instance.humanRacers[playerId].Stats();
	}

	private void Tick() {
		currentTime = Mathf.Max(currentTime - Time.deltaTime, 0.0f);
	}

	private bool DoneWaiting() {
		return Mathf.Approximately(currentTime, 0.0f);
	}

	private void SpawnRacers() {
		int pos = 0;

		for( int i = 0; i < MaxPlayers; ++i ) {
			bool isActive = playerProfiles[i] != null;
			if( isActive ) {
				var obj = Spawn(humanPrefabs[i], spawnPoints[pos++]);
				humanRacers[i] = obj.GetComponent<RacePlayer>();
				AssignCamera(i, obj);

			}
			playerViews[i].IsEnabled = isActive;
		}
		for( int i = 0; i < GhostCount; ++i ) {
			var obj = Spawn(ghostPrefabs[i], spawnPoints[pos++]);
			ghostRacers[i] = obj.GetComponent<RacePlayer>();
		}
	}

	private void AssignCamera(int index, GameObject parent) {
		var cameraAnchor = parent.GetComponentInChildren<HorseController>().transform;
		cameras[index].Target = cameraAnchor;
		cameras[index].Camera.enabled = true;
	}


	private GameObject Spawn(GameObject obj, Transform transform) {
		return Instantiate(obj, transform.position, transform.rotation);
	}

	private void Clear() {
		for( int i = 0; i < MaxPlayers; ++i )
			playerProfiles[i] = null;
	}
}