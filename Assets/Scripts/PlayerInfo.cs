using UnityEngine;

public class PlayerInfo {
	public string Name { get; private set; }
	public bool IsPlaying { get; private set; }

	public static PlayerInfo Idle = new PlayerInfo("", false);

	public PlayerInfo(string name, bool isPlaying = true) {
		Name = name;
		IsPlaying = isPlaying;
	}
}