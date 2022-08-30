using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Lobby : MonoBehaviour {
	[SerializeField]
	LobbyPlayer[] players;

	private static readonly System.Random rng = new System.Random();

	[SerializeField]
	private Timer timer;

	void Start() {
		timer.Reset();
		var startingNames = GetStartingNames(players.Length);

		for( int i = 0; i < players.Length; ++i )
			players[i].Name = startingNames[i];
	}

	void Update() {
		for( int i = 0; i < players.Length; ++i )
			UpdatePlayer(i);

		bool playersReady = players.Count(p => p.IsConnected) > 0
			&& players.All(p => p.IsReady || !p.IsConnected);

		if( playersReady && !timer.IsRunning ) {
			timer.Show(true);
			timer.Begin();
		}
		else if( !playersReady && timer.IsRunning ) {
			timer.Show(false);
			timer.Reset();
		}

		if( timer.IsDone )
			Debug.Log("Timer is done!");
	}

	private void UpdatePlayer(int playerId) {
		if( InputController.GetStartDown(playerId) ) {
			if( players[playerId].IsConnected )
				players[playerId].IsReady = !players[playerId].IsReady;
			else
				players[playerId].IsConnected = true;
		}
		if( InputController.GetCancelDown(playerId) ) {
			players[playerId].IsConnected = false;
			players[playerId].IsReady = false;
		}
	}

	private static IList<string> GetStartingNames(int count) {
		var names = GetGenericNames();
		var indices = new List<int>(count);

		while( indices.Count < count ) {
			int index = rng.Next(names.Count);
			do
				index = (index + 1) % names.Count;
			while( indices.Contains(index) );
			indices.Add(index);
		}

		var picks = new List<string>(count);

		foreach( int index in indices )
			picks.Add(names[index]);

		return picks;
	}

	private static IList<string> GetGenericNames() {
		return new List<string>() {
			"2 Fast 2 Furious",
			"2 Good 2 Kart",
			"2-Stroke 2-Furious",
			"A la Kart",
			"Aaron Karter",
			"Accelerators",
			"Ace of Race",
			"Adrenaline",
			"Alpha",
			"Amazing Race",
			"Are We There Yet?",
			"Axle",
			"Baby Formula 1",
			"Blade Runner",
			"Blazin’ Wheels",
			"Blitz",
			"Boomer",
			"Brake Dancers",
			"Britney Steers",
			"Brute Force",
			"Burnout",
			"Captain Amerikart",
			"Carnage",
			"Chain Lightning",
			"Chicane Master",
			"Chick Magnet",
			"Circuit Breaker",
			"Clutch This!",
			"Count Dragula",
			"Crazy Karter",
			"Daredevil",
			"Dominator",
			"Eat My Dust",
			"Eat, Sleep, Kart",
			"Eliminator",
			"Fast and Furious",
			"Fast Five",
			"Fast, Faster, Me!",
			"Fearless",
			"Finisher",
			"Firebrand",
			"Flare Intake System",
			"Formula None",
			"Forza",
			"From Last to First",
			"Fueled Up",
			"Full Throttle",
			"Furiosa",
			"G-Force",
			"Geared to Win",
			"Get Smoked!",
			"Getaway",
			"Ghost Rider",
			"Go Kart or Go Home",
			"Gone in Sixty Seconds",
			"Gone with the Wind",
			"Grand Stand",
			"Grease Monkey",
			"Grim Reaper",
			"GTA",
			"High Speed",
			"High Voltage",
			"Hold My Beer",
			"Horse Power",
			"Hot Engine",
			"Hot Wheels",
			"How’s My Rear View?",
			"Hurricane",
			"I’m too Young to Finish",
			"Last",
			"Indestructible",
			"Inferno",
			"Invincible",
			"Jurassic Kart",
			"Kart Attack",
			"Kart Crusher",
			"Kart Killer",
			"Kart Master",
			"Kart Wars",
			"Kart-o-maniac",
			"Kart2Kart",
			"Kartatouille",
			"Karterina",
			"Kartman",
			"Kartmobile",
			"Kartographer",
			"Kartology",
			"Karton",
			"Kartridge",
			"Kartuga",
			"Karty Perry",
			"Kartzilla",
			"Kickin’ Assphalt",
			"King of the Thrill",
			"King of the Track",
			"Klutch",
			"Knockout",
			"License to Thrill",
			"Light Speed",
			"Mad Max",
			"Magna Karta",
			"Make Haste",
			"Mario Kart",
			"Max Power",
			"Max Speed",
			"Maximus",
			"Mc Driver",
			"Me First, You Last",
			"Mean Machine",
			"Melting Rubber",
			"Most Wanted",
			"My Kart > Your Kart",
			"Naskart",
			"Need for Speed",
			"Nicholas RollCage",
			"Nitro",
			"NOS Machine",
			"Octane",
			"Optimus Prime",
			"Overlap",
			"Ownage",
			"Pacer",
			"Passing Through!",
			"Phantom",
			"Pist n’ Broke",
			"Piston Head",
			"Pole Position",
			"Prowler",
			"Pumped Up Kits",
			"R/C Kart",
			"Race Ventura",
			"Racing Addict",
			"Raise The Bar",
			"Ready, Set, Win",
			"Redline",
			"Roadrunner",
			"Robert DeGearo",
			"Robert Downshift Jr",
			"Robo Kart",
			"Rocket-Bye-Baby",
			"Screechers",
			"Shooting Star",
			"Shopping Kart",
			"Skull Crusher",
			"Slick",
			"Smoked",
			"Smokin’ Tires",
			"Smokin’ Wheels",
			"Spark Plug",
			"Specific Rim",
			"Speed Demon",
			"Speed Maniac",
			"Speed Racer",
			"Speedin’ Demon",
			"Speedometer",
			"Speedsters",
			"Speedy Gonzales",
			"Spin Diesel",
			"Spin to Win",
			"Spinout",
			"Sprint King",
			"Sprocket Rocket",
			"Steervester Stallone",
			"Step on It",
			"Steven Squeelberg",
			"Stinger",
			"Stomper",
			"T-Wrecks",
			"Taylor Drift",
			"The Avenger",
			"The Checkered Flags",
			"The Fast and The Furious",
			"The Kartaracs",
			"The Outlaw",
			"The Overtaker",
			"The Pedal To The Metal",
			"The Punisher",
			"The Spartan",
			"The Wind",
			"This is Kartaaa!",
			"To Kart or Not to Kart",
			"Tokyo Drift",
			"Tour Guide",
			"Track Beast",
			"Track Police",
			"Trailblazer",
			"Training Wheels",
			"Transporter",
			"Trojan Kart",
			"Troublemaker",
			"Turbo",
			"Turbomeister",
			"United States of Amerikart",
			"USS Karterprise",
			"Viper",
			"Warlock",
			"Where’d He Go?",
			"Where’s the Finish Line?",
			"Will Race for Food",
			"XXL",
			"Yes, Another Lap",
			"You Snooze, You Lose",
			"ZigZag",
			"Zoomer",
			"Zorro",
		};
	}
}