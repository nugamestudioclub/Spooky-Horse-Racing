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
		return Path + "horses" + id + ".txt";
	}

	private static StreamReader OpenHorseReader(int id) {
		var path = GetHorsePath(id);

		if( !File.Exists(path) )
			File.Create(Path);

		return new StreamReader(path);
	}

	public static IEnumerable<SerializableTransformData> ReadHorseData(int id) {
		using var reader = OpenHorseReader(id);
		string line;

		while( (line = reader.ReadLine()) != null )
			yield return JsonUtility.FromJson<SerializableTransformData>(line);
	}

	private static StreamWriter OpenHorseWriter(int id) {
		var path = GetHorsePath(id);

		if( !File.Exists(path) )
			File.Create(Path);

		return new StreamWriter(path);
	}

	public static void WriteHorseData(int id, IEnumerable<SerializableTransformData> data) {
		using var writer = OpenHorseWriter(id);

		foreach( var item in data )
			writer.Write(JsonUtility.ToJson(item));
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
	public string time;
	public int place;
	public int hitCount;
	public int coinCount;
	public int knightId;
	public int horseId;
}