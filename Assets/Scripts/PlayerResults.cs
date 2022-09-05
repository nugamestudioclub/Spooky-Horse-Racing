public class PlayerResults {
	public string name;

	public int place;
	public bool isPlaceBest;

	public float time;
	public bool isTimeBest;

	public int hitCount;
	public bool isHitsBest;

	public int coinCount;
	public bool isCoinsBest;

	public static readonly PlayerResults Default = new PlayerResults("", place:0, time:0.0f, hitCount:0, coinCount:0);

	public PlayerResults(string name,
		int place, float time, int hitCount, int coinCount,
		bool isPlaceBest = false, bool isTimeBest = false, bool isHitsBest = false, bool isCoinsBest = false
	) {
		this.name = name;
		this.place = place;
		this.isPlaceBest = isPlaceBest;
		this.time = time;
		this.isTimeBest = isTimeBest;
		this.hitCount = hitCount;
		this.isHitsBest = isHitsBest;
		this.coinCount = coinCount;
		this.isCoinsBest = isCoinsBest;
	}
}