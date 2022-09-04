public class PlayerResults {
	public string name;

	public int place;
	public bool isPlaceBest;

	public float time;
	public bool isTimeBest;

	public int hitsCount;
	public bool isHitsBest;

	public int coinsCount;
	public bool isCoinsBest;

	public static readonly PlayerResults Default = new PlayerResults("", place:0, time:0.0f, hitsCount:0, coinsCount:0);

	public PlayerResults(string name,
		int place, float time, int hitsCount, int coinsCount,
		bool isPlaceBest = false, bool isTimeBest = false, bool isHitsBest = false, bool isCoinsBest = false
	) {
		this.name = name;
		this.place = place;
		this.isPlaceBest = isPlaceBest;
		this.time = time;
		this.isTimeBest = isTimeBest;
		this.hitsCount = hitsCount;
		this.isHitsBest = isHitsBest;
		this.coinsCount = coinsCount;
		this.isCoinsBest = isCoinsBest;
	}
}