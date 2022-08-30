using UnityEngine.SceneManagement;

public static class SceneController {
	public static void LoadLobby() {
		SceneManager.LoadScene("Lobby");
	}

	public static void LoadRace() {
		SceneManager.LoadScene("RollingScene"); ///
	}
}