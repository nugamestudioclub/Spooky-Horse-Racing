using System.Linq;
using UnityEngine;

public class Results : MonoBehaviour {
	[SerializeField]
	private ResultsPlayerView[] players = new ResultsPlayerView[Race.MaxRacers];

	void Start() {
		int totalRacers = Race.TotalRacers;

		for( int i = 0; i < totalRacers; ++i )
			players[i].Draw(Race.GetPlayerResults(i));
		for( int i = totalRacers; i < Race.MaxRacers; ++i )
			players[i].IsVisible = false;
	}

	void Update() {
		if( Race.GetActiveIds().Any(id => InputController.GetStartDown(id)) )
			SceneController.LoadLobby();
	}
}