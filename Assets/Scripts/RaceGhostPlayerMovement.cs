using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RaceGhostPlayerMovement : RacePlayerMovement {
	public HorseRecordingState state;

	private int readPosition;

	private bool EndOfFile => readPosition >= data.Count;

	SerializableTransformData NextData() {
		return data.Count == 0
			? new SerializableTransformData()
			: data[readPosition++];
	}

	private IList<SerializableTransformData> data;

	SerializableTransformData currentData;

	private void FixedUpdate() {
		if( !ControlEnabled )
			return;

		if( state == HorseRecordingState.Playback )
			Playback();
		else if( state == HorseRecordingState.Record )
			Record();
	}

	public override void Play() {
		if( state == HorseRecordingState.Playback )
			data = new List<SerializableTransformData>(Database.ReadHorseData(ControllerId));
		else if( state == HorseRecordingState.Record )
			data = new List<SerializableTransformData>();
	}

	public override void Stop() {
		Database.WriteHorseData(ControllerId, data);
	}

	private void Playback() {
		if( EndOfFile )
			return;

		transform.position = Vector3.Lerp(
			transform.position,
			new Vector3(currentData.pX, currentData.pY, currentData.pZ),
			1.0f
		);
		transform.eulerAngles = new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y,
			currentData.rot
		);

		if( Vector2.Distance(new Vector2(currentData.pX, currentData.pY), transform.position) < 0.1f )
			currentData = NextData();
	}

	private void Record() {
		data.Add(new SerializableTransformData {
			pX = transform.position.x,
			pY = transform.position.y,
			pZ = transform.position.z,
			rot = transform.eulerAngles.z
		});
	}

	public override void Freeze() { }

	public override void Freeze(float duration) { }

	public override void UnFreeze() { }
}

public enum HorseRecordingState { Record, Playback };