using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Database {

	private static string path;
	public static string Path {
		get {
			if( string.IsNullOrEmpty(path) )
				path = Application.dataPath + @"\Resources\";
			return path;
		}
		private set {
			path = value;
		}
	}

	private static string GetHorsePath(int id) {
		return Path + "horses" + id + ".json";
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
			File.Create(Path);

		using var writer = new StreamWriter(path);

		foreach( var item in data )
			writer.Write(JsonUtility.ToJson(item));
	}

	private static string GetBestPath(int id) {
		return Path + "best" + id + ".json";
	}

	public static SerializableBestData ReadBestData(int id) {
		string path = GetBestPath(id);

		if( !File.Exists(path) )
			return new SerializableBestData();
		else
			return JsonUtility.FromJson<SerializableBestData>(path);
	}

	public static void WriteBestData(int id, SerializableBestData data) {
		string path = GetBestPath(id);

		if( !File.Exists(path) )
			File.Create(path);

		using var writer = new StreamWriter(path);

		writer.Write(JsonUtility.ToJson(data));
	}
}

[Serializable]
public struct SerializableTransformData {
	public float pX;
	public float pY;
	public float pZ;
	public float rot;
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
		place: 1,
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
		knightId:0,
		horseId:0
	) { }
}