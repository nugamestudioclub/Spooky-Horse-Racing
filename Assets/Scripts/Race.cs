using UnityEngine;

public class Race : MonoBehaviour {
	public static readonly int PlayerCount = 4;

	private static Race instance;

	private readonly PlayerInfo[] playerInfo = new PlayerInfo[PlayerCount];

	void Awake() {
		if( instance == null ) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public static void Register(int playerId, PlayerInfo playerInfo) {
		instance.playerInfo[playerId] = playerInfo;
		Debug.Log($"{playerId}: {playerInfo.Name}");
	}
}