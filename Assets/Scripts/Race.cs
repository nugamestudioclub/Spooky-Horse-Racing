using System;
using System.Collections.Generic;
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
	public static readonly int MaxHumanPlayers = 4;

	public static readonly int MaxRacers = 8;

	public int ActiveHumanPlayers => playerProfiles.Count(x => x != null);

	public static readonly int GhostCount = 4;

	public static int TotalRacers => (instance == null ? 0 : instance.ActiveHumanPlayers) + GhostCount;

	private static Race instance;

	[SerializeField]
	private CameraFollow[] cameras = new CameraFollow[MaxHumanPlayers];

	[SerializeField]
	private RaceState state;

	[Range(0.0f, float.MaxValue)]
	[SerializeField]
	private float delayTime;

	private float currentTime;

	[SerializeField]
	private Timer timer;

	private static readonly PlayerProfile[] playerProfiles = new PlayerProfile[MaxRacers];

	private static readonly PlayerResults[] playerResults = new PlayerResults[MaxRacers];

	[SerializeField]
	private Transform[] spawnPoints = new Transform[MaxRacers];

	[SerializeField]
	private GameObject[] humanPrefabs = new GameObject[MaxHumanPlayers];

	[SerializeField]
	private GameObject[] ghostPrefabs = new GameObject[GhostCount];

	private readonly RacePlayer[] humanRacers = new RacePlayer[MaxHumanPlayers];

	private readonly RacePlayer[] ghostRacers = new RacePlayer[GhostCount];

	[SerializeField]
	private RacePlayerView[] playerViews = new RacePlayerView[MaxHumanPlayers];

	private bool done;

	void Awake() {
		instance = this;
		Initialize();
	}

	void Start() {
		SpawnRacers();
	}

	void Update() {
		switch( state ) {
		case RaceState.Waiting: WaitingUpdate(); break;
		case RaceState.Countdown: CountdownUpdate(); break;
		case RaceState.Racing: RacingUpdate(); break;
		case RaceState.Finished: FinishedUpdate(); break;
		}
	}

	private void Initialize() {
		currentTime = delayTime;
		state = RaceState.Waiting;
		foreach( CameraFollow camera in cameras ) {
			camera.Camera.enabled = false;
		}
	}

	private void WaitingUpdate() {
		currentTime = Mathf.Max(currentTime - Time.deltaTime, 0.0f);
		if( DoneWaiting() ) {
			timer.Show(true);
			timer.Begin();
			state = RaceState.Countdown;
		}
	}

	private void CountdownUpdate() {
		if( timer.IsDone ) {
			timer.Show(false);
			timer.Reset();
			state = RaceState.Racing;
		}
	}

	private void RacingUpdate() {
		foreach( var racer in humanRacers )
			if( racer != null )
				racer.Time += Time.deltaTime;
		foreach( var racer in ghostRacers )
			if( racer != null )
				racer.Time += Time.deltaTime;
		// state = RaceState.Finished;
	}

	private void FinishedUpdate() {
		if( done ) {
			SceneController.LoadResults();
		}
		else {
			var racers = GetAllRacers().ToList();
			var results = new List<PlayerResults>(racers.Count);

			for( int i = 0; i < racers.Count; ++i )
				results.Add(MakePlayerResults(GetPlayerProfile(i).Name, GetPlayerStats(i)));
			results.Sort((a, b) => a.place != 0 && a.place < b.place ? -1 : 1);
			results.CopyTo(playerResults);

			Clear();
			done = true;
		}
	}

	public static void Register(int playerId, PlayerProfile playerProfile) {
		playerProfiles[playerId] = playerProfile;
	}

	public IEnumerable<RacePlayer> GetHumanRacers() {
		foreach( var human in humanRacers )
			if( human != null )
				yield return human;
	}

	public IEnumerable<RacePlayer> GetGhostRacers() {
		foreach( var ghost in ghostRacers )
			if( ghost != null )
				yield return ghost;
	}

	public IEnumerable<RacePlayer> GetAllRacers() {
		foreach( var human in GetHumanRacers() )
			yield return human;
		foreach( var ghost in GetGhostRacers() )
			yield return ghost;
	}

	public static PlayerProfile GetPlayerProfile(int playerId) {
		return playerProfiles[playerId] ?? PlayerProfile.Idle;
	}

	public static PlayerStats GetPlayerStats(int playerId) {
		var racer = playerId < MaxHumanPlayers
			? instance.humanRacers[playerId]
			: instance.ghostRacers[playerId - MaxHumanPlayers];

		return racer == null
			? new PlayerStats()
			: racer.Stats();
	}

	private PlayerResults MakePlayerResults(string name, PlayerStats stats) {
		return new PlayerResults(
			name,
			stats.place,
			stats.time,
			stats.hitCount,
			stats.coinCount,
			CheckBestPlace(stats),
			CheckBestTime(stats),
			CheckBestHits(stats),
			CheckBestCoins(stats)
		);
	}

	public static PlayerResults GetPlayerResults(int playerId) {
		return playerResults[playerId] ?? PlayerResults.Default;
	}

	private bool DoneWaiting() {
		return Mathf.Approximately(currentTime, 0.0f);
	}

	private void SpawnRacers() {
		int pos = 0;

		for( int i = 0; i < MaxHumanPlayers; ++i ) {
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

	private bool CheckBestPlace(PlayerStats stats) {
		return !stats.isGhost && stats.place == 1;
	}

	private bool CheckBestTime(PlayerStats stats) {
		return !stats.isGhost; ///
	}

	private bool CheckBestHits(PlayerStats stats) {
		return !stats.isGhost; ///
	}

	private bool CheckBestCoins(PlayerStats stats) {
		return !stats.isGhost; ///
	}

	private void Clear() {
		for( int i = 0; i < MaxHumanPlayers; ++i )
			playerProfiles[i] = null;
	}
}