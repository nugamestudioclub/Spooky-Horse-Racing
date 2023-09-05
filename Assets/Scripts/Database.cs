using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public struct SerializableTransformData {
	public float pX;
	public float pY;
	public float pZ;
	public float rot;
}

public enum BestCategory {
	Place,
	Time,
	Hits,
	Coins,
}

[Serializable]
public class SerializableBestData {
	public string name;
	public float time;
	public int place;
	public int hitCount;
	public int coinCount;
	public int knightId;
	public int horseId;

	public SerializableBestData() : this(
		name: "Anonymous",
		time: float.MaxValue,
		place: int.MaxValue,
		hitCount: 0,
		coinCount: 0,
		knightId: 0,
		horseId: 0
	) { }

	public SerializableBestData(
		string name,
		float time,
		int place,
		int hitCount,
		int coinCount,
		int knightId,
		int horseId
	) {
		this.name = name;
		this.time = time;
		this.place = place;
		this.hitCount = hitCount;
		this.coinCount = coinCount;
		this.knightId = knightId;
		this.horseId = horseId;
	}

	public SerializableBestData(PlayerProfile profile, PlayerStats stats) : this(
		profile.Name,
		stats.time,
		stats.place,
		stats.hitCount,
		stats.coinCount,
		knightId: 0, ///
	  horseId: 0 ///
 ) { }
}

public static class Database {

	private static string rootPath;
	public static string RootPath {
		get {
			if( string.IsNullOrEmpty(rootPath) )
				rootPath = Application.dataPath + @"\Resources\";
			return rootPath;
		}
		private set {
			rootPath = value;
		}
	}

	private static string GetHorsePath(int id) {
		return RootPath + "horses" + id + ".json";
	}

	public static IEnumerable<SerializableTransformData> ReadHorseData(int id) {
		string path = GetHorsePath(id);

		if( !File.Exists(path) )
			yield break;

		using var reader = new StreamReader(path);
		string line;

		while( (line = reader.ReadLine()) != null )
			yield return JsonUtility.FromJson<SerializableTransformData>(line);
	}

	public static void WriteHorseData(int id, IEnumerable<SerializableTransformData> data) {
		string path = GetHorsePath(id);

		if( !File.Exists(path) )
			File.Create(path);

		using var writer = new StreamWriter(path);

		try {
			foreach( var item in data )
				writer.WriteLine(JsonUtility.ToJson(item));
		}
		catch( Exception ex ) {
			Debug.Log(ex.Message);
		}
	}

	private static string GetBestPath(BestCategory category) {
		return RootPath + "best_" + Enum.GetName(typeof(BestCategory), category).ToLower() + ".json";
	}

	public static SerializableBestData ReadBestData(BestCategory category) {
		string path = GetBestPath(category);

		if( !File.Exists(path) )
			return new SerializableBestData();

		using var reader = new StreamReader(path);

		try {
			string line = reader.ReadLine();
			return JsonUtility.FromJson<SerializableBestData>(line);
		}
		catch( Exception ex ) {
			Debug.Log(ex.Message);
			return new SerializableBestData();
		}
	}

	public static void WriteBestData(BestCategory category, SerializableBestData data) {
		string path = GetBestPath(category);

		if( !File.Exists(path) )
			File.Create(path);

		using var writer = new StreamWriter(path);

		try {
			writer.WriteLine(JsonUtility.ToJson(data));
		}
		catch( Exception ex ) {
			Debug.Log(ex.Message);
		}
	}
}