using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraCulling : MonoBehaviour {
	[SerializeField]
	private int id;

	void Start() {
		var camera = GetComponent<Camera>();

		foreach( int i in Enumerable.Range(0, Race.MaxRacers) )
			camera.cullingMask &= ~(1 << LayerMask.NameToLayer(
				(i == id ? "Not_Player_" : "Player_") + id
			));
	}
}