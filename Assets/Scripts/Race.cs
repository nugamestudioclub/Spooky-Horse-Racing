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

public class CheckpointStatus {
	public RacePlayer racer;
	public int checkpoint;

	public CheckpointStatus(RacePlayer racer, int checkpoint) {
		this.racer = racer;
		this.checkpoint = checkpoint;
	}
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

	private static readonly SerializableBestData[] bestData = new SerializableBestData[GhostCount];

	[SerializeField]
	private RacePlayerView[] playerViews = new RacePlayerView[MaxHumanPlayers];

	[SerializeField]
	private LineRenderer checkpointLineRenderer;

	private Vector3[] checkpoints;

	private bool done;

	void Awake() {
		instance = this;
		Initialize();
	}

	void Start() {
		SpawnRacers();
		foreach( var racer in GetAllRacers() ) {
			racer.ControlEnabled = false;
		}
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

		checkpoints = new Vector3[checkpointLineRenderer.positionCount];
		checkpointLineRenderer.GetPositions(checkpoints);

		ReadBestData();
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
			StartRecording();
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
		var racers = GetRacersInOrder();

		for( int i = 0; i < racers.Count; ++i ) {
			// Debug.Log($"racer {racers[i].Id}: {Strings.ToOrdinal(i + 1)}");
			racers[i].Place = i + 1;
		}
	}

	private void FinishedUpdate() {
		if( done ) {
			SceneController.LoadResults();
		}
		else {
			StopRecording();
			HandleResults();
			WriteBestData();

			Clear();
			done = true;
		}
	}

	public static IEnumerable<int> GetActiveIds() {
		for( int i = 0; i < active.Length; ++i )
			if( active[i] )
				yield return i;
	}

	public RacePlayer GetRacer(int playerId) {
		return playerId < MaxHumanPlayers ? humanRacers[playerId] : ghostRacers[playerId];
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

	private PlayerResults MakePlayerResults(PlayerProfile profile, PlayerStats stats) {
		/*
		return new PlayerResults(
			profile.Name,
			stats.place,
			stats.time,
			stats.hitCount,
			stats.coinCount,
			CheckBestPlace(profile, stats),
			CheckBestTime(profile, stats),
			CheckBestHits(profile, stats),
			CheckBestCoins(profile, stats)
		);
		*/
		int isGoodTime = FindBestTime(profile, stats);

		return new PlayerResults(
			profile.Name,
			stats.place,
			stats.time,
			stats.hitCount,
			stats.coinCount,
			CheckBestPlace(profile, stats),
			isGoodTime == 1,
			isGoodTime == 2,
			isGoodTime == 3
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

		for( int i = 0; i < MaxHumanPlayers; ++i )
			if( cameras[i].Target == null )
				AssignCamera(i, ghostRacers[i].gameObject);

		TotalRacers = ActiveHumanPlayers + GhostCount;
	}

	private GameObject Spawn(GameObject obj, Transform transform) {
		return Instantiate(obj, transform.position, transform.rotation);
	}

	private void LoadHuman(int index, int position) {
		var obj = Spawn(humanPrefabs[index], spawnPoints[position]);
		var racer = obj.GetComponent<RacePlayer>();
		var profile = GetPlayerProfile(index);

		racer.SetController(index);
		racer.Name = profile.Name;
		racer.Id = index;
		racer.IsGhost = false;
		racer.Place = position + 1;

		if( profile.Knight != null )
			racer.Knight = profile.Knight;

		if( profile.Horse != null )
			racer.Horse = profile.Horse;

		AssignCamera(index, obj);

		humanRacers[index] = racer;

	}

	private void LoadGhost(int index, int position) {
		var obj = Spawn(ghostPrefabs[index], spawnPoints[position]);
		var racer = obj.GetComponent<RacePlayer>();
		int id = index + MaxHumanPlayers;
		var data = bestData[index];

		racer.SetController(id);
		racer.Name = "Ghost of " + data.name;
		racer.Id = id;
		racer.IsGhost = true;
		racer.Place = position + 1;

		/*
		if( profile.Knight != null )
			racer.Knight = profile.Knight;

		if (profile.Horse != null)
			racer.Horse = profile.Horse;
		*/

		ghostRacers[index] = racer;
	}

	private void AssignCamera(int index, GameObject parent) {
		var cameraAnchor = parent.GetComponentInChildren<HorseController>().transform;
		cameras[index].Target = cameraAnchor;
		cameras[index].Camera.enabled = true;
	}

	private bool CheckBestPlace(PlayerProfile profile, PlayerStats stats) {
		if( !stats.isGhost && stats.place == 1 ) {
			SetBestData(BestCategory.Place, new SerializableBestData(profile, stats));
			return true;
		}
		else {
			return false;
		}
	}

	private bool CheckBestTime(PlayerProfile profile, PlayerStats stats) {
		var best = Database.ReadBestData(BestCategory.Time);

		if( !stats.isGhost && stats.time <= best.time ) {
			SetBestData(BestCategory.Time, new SerializableBestData(profile, stats));
			return true;
		}
		else {
			return false;
		}
	}

	private bool CheckBestHits(PlayerProfile profile, PlayerStats stats) {
		var best = Database.ReadBestData(BestCategory.Hits);

		if( !stats.isGhost && stats.hitCount >= best.hitCount ) {
			SetBestData(BestCategory.Hits, new SerializableBestData(profile, stats));
			return true;
		}
		else {
			return false;
		}
	}

	private bool CheckBestCoins(PlayerProfile profile, PlayerStats stats) {
		var best = Database.ReadBestData(BestCategory.Coins);

		if( !stats.isGhost && stats.coinCount >= best.coinCount ) {
			SetBestData(BestCategory.Coins, new SerializableBestData(profile, stats));
			return true;
		}
		else {
			return false;
		}
	}

	private int FindBestTime(PlayerProfile profile, PlayerStats stats) {
		var list = bestData.ToList();
		int index = list.FindIndex(0, x => x.time > stats.time);

		if( index > 0 ) {
			list.RemoveAt(list.Count - 1);
			list.Insert(index, new SerializableBestData(profile, stats));
			list.CopyTo(bestData);
		}
		return index;
	}

	private bool IsDoneWaiting() {
		return Mathf.Approximately(currentTime, 0.0f);
	}
	private bool IsGameOver() {
		return GetHumanRacers().All(x => x.HasReachedFinishLine);
	}

	private void Clear() {
		for( int i = 0; i < MaxHumanPlayers; ++i )
			playerProfiles[i] = null;
	}

	private int FindNearestCheckpoint(RacePlayer racer) {
		var pairs = Enumerable.Range(0, checkpoints.Length)
			.Select(i => new KeyValuePair<int, float>(
				i,
				Vector3.Distance(checkpoints[i], racer.Transform.position))
			)
			.ToList();
		pairs.Sort((a, b) => a.Value.CompareTo(b.Value));
		int index = pairs[0].Key;

		if( index == 0 || index == checkpoints.Length - 1 )
			index = racer.HasReachedMidpoint ? checkpoints.Length - 1 : 0;

		return index;
	}

	private CheckpointStatus GetCheckpointStatus(RacePlayer racer) {
		return new CheckpointStatus(racer, FindNearestCheckpoint(racer));
	}

	private int CompareCheckpointStatus(CheckpointStatus first, CheckpointStatus second) {
		if( first.racer.HasReachedFinishLine && second.racer.HasReachedFinishLine ) {
			return first.racer.Place.CompareTo(second.racer.Place);
		}
		else if( first.racer.HasReachedFinishLine && !second.racer.HasReachedFinishLine ) {
			return -1;
		}
		else if( !first.racer.HasReachedFinishLine && second.racer.HasReachedFinishLine ) {
			return 1;
		}
		else if( first.checkpoint > second.checkpoint ) {
			return -1;
		}
		else {
			var nextCheckpoint = checkpoints[Math.Min(first.checkpoint + 1, checkpoints.Length - 1)];
			return Vector3.Distance(nextCheckpoint, first.racer.Transform.position)
				.CompareTo(Vector3.Distance(nextCheckpoint, second.racer.Transform.position));
		}
	}

	private IList<RacePlayer> GetRacersInOrder() {
		var racers = GetAllRacers()
			.Select(x => GetCheckpointStatus(x))
			.ToList();
		racers.Sort((a, b) => CompareCheckpointStatus(a, b));
		return racers.Select(x => x.racer).ToList();
	}

	private void StartRecording() {
		foreach( var racer in GetAllRacers() ) {
			racer.ControlEnabled = true;
			racer.Recording.ControlEnabled = true;
			racer.Recording.Play();
		}
	}

	private void StopRecording() {
		foreach( var racer in GetAllRacers() ) {
			racer.Recording.Stop();
			racer.Recording.ControlEnabled = false;
			racer.ControlEnabled = false;
		}
	}

	private void HandleResults() {
		var results = Enumerable.Range(0, MaxRacers)
				.Select(i => MakePlayerResults(GetPlayerProfile(i), GetPlayerStats(i)))
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
	}

	private void ReadBestData() {
		foreach( BestCategory category in Enum.GetValues(typeof(BestCategory)) ) {
			SetBestData(category, Database.ReadBestData(category));
		}
	}

	private void WriteBestData() {
		for( int i = 0; i < MaxHumanPlayers; ++i ) {
			var racer = humanRacers[i];
			if( racer != null ) {
				var results = playerResults[i];
				if( results.isPlaceBest )
					WriteNewBest(racer, BestCategory.Place);
				if( results.isTimeBest )
					WriteNewBest(racer, BestCategory.Time);
				if( results.isHitsBest )
					WriteNewBest(racer, BestCategory.Hits);
				if( results.isCoinsBest )
					WriteNewBest(racer, BestCategory.Coins);
			}
		}
	}

	private void WriteNewBest(RacePlayer racer, BestCategory category) {
		if( racer.Recording != null && racer.Recording.Data != null ) {
			Database.WriteHorseData((int)category + GhostCount, racer.Recording.Data);
			Database.WriteBestData(category, GetBestData(category));
		}
	}

	private SerializableBestData GetBestData(BestCategory category) {
		return bestData[(int)category];
	}

	private void SetBestData(BestCategory category, SerializableBestData data) {
		bestData[(int)category] = data;
	}
}