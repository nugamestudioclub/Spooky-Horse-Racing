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

	public int ActiveHumanPlayers { get; private set; }

	public static readonly int GhostCount = 4;

	public static int TotalRacers { get; private set; } = GhostCount;

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

	private static readonly bool[] active = new bool[MaxHumanPlayers];

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
		foreach( var racer in GetAllRacers() )
			racer.ControlEnabled = false;
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
		for( int i = 0; i < active.Length; ++i )
			active[i] = playerProfiles[i] != null;
		foreach( CameraFollow camera in cameras )
			camera.Camera.enabled = false;
		state = RaceState.Waiting;
	}

	private void WaitingUpdate() {
		currentTime = Mathf.Max(currentTime - Time.deltaTime, 0.0f);
		if( IsDoneWaiting() ) {
			timer.Show(true);
			timer.Begin();
			state = RaceState.Countdown;
		}
	}

	private void CountdownUpdate() {
		if( timer.IsDone ) {
			timer.Show(false);
			timer.Reset();
			foreach( var racer in GetAllRacers() )
				racer.ControlEnabled = true;
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
		if( IsGameOver() )
			state = RaceState.Finished;
	}

	private void FinishedUpdate() {
		if( done ) {
			SceneController.LoadResults();
		}
		else {
			foreach( var racer in GetAllRacers() )
				racer.ControlEnabled = false;

			var results = Enumerable.Range(0, MaxRacers)
				.Select(i => MakePlayerResults(GetPlayerProfile(i).Name, GetPlayerStats(i)))
				.ToList();

			results.Sort((a, b) => {
				if( a.place <= 0 && b.place > 0 )
					return 1;
				else if( a.place > 0 && b.place <= 0 )
					return -1;
				else
					return a.place.CompareTo(b.place);
			});

			results.CopyTo(playerResults);

			Clear();
			done = true;
		}
	}

	public static IEnumerable<int> GetActiveIds() {
		for( int i = 0; i < active.Length; ++i )
			if( active[i] )
				yield return i;
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

	public static void Register(int playerId, PlayerProfile playerProfile) {
		playerProfiles[playerId] = playerProfile;
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

	private void SpawnRacers() {
		int pos = 0;

		ActiveHumanPlayers = 0;
		for( int i = 0; i < MaxHumanPlayers; ++i ) {
			if( active[i] ) {
				LoadHuman(i, pos++);
				++ActiveHumanPlayers;
			}
			playerViews[i].IsEnabled = active[i];
		}
		for( int i = 0; i < GhostCount; ++i ) {
			LoadGhost(i, pos++);
		}

		TotalRacers = ActiveHumanPlayers + GhostCount;
	}

	private GameObject Spawn(GameObject obj, Transform transform) {
		return Instantiate(obj, transform.position, transform.rotation);
	}

	private void LoadHuman(int index, int position) {
		var obj = Spawn(humanPrefabs[index], spawnPoints[position]);
		var racer = obj.GetComponent<RacePlayer>();

		racer.SetController(index);
		racer.Name = GetPlayerProfile(index).Name;
		racer.Id = index;
		racer.IsGhost = false;
		racer.Place = position + 1;

		AssignCamera(index, obj);

		humanRacers[index] = racer;

	}

	// needs to load ghost data from database
	private void LoadGhost(int index, int position) {
		var obj = Spawn(humanPrefabs[index], spawnPoints[position]);
		var racer = obj.GetComponent<RacePlayer>();
		int id = index + MaxHumanPlayers;

		// racer.SetController(-1);
		racer.Name = GetPlayerProfile(id).Name;
		racer.Id = id;
		racer.IsGhost = true;
		racer.Place = position + 1;

		ghostRacers[index] = racer;
	}

	private void AssignCamera(int index, GameObject parent) {
		var cameraAnchor = parent.GetComponentInChildren<HorseController>().transform;
		cameras[index].Target = cameraAnchor;
		cameras[index].Camera.enabled = true;
	}

	// these check functions should read from database
	// and update if there is a new high score
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

	private bool IsDoneWaiting() {
		return Mathf.Approximately(currentTime, 0.0f);
	}
	private bool IsGameOver() {
		return false; ///
	}

	private void Clear() {
		for( int i = 0; i < MaxHumanPlayers; ++i )
			playerProfiles[i] = null;
	}
}