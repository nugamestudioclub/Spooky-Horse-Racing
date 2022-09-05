#if UNITY_EDITOR

using UnityEngine;

class EditorSphere : MonoBehaviour {
	[Range(0.0f, float.MaxValue)]
	[SerializeField]
	private float radius = 1.0f;

	[SerializeField]
	private Color color = Color.white;

	[SerializeField]
	private bool isWireframe;

	private SpriteRenderer spriteRenderer = null;

	void OnDrawGizmos() {
		Gizmos.color = color;
		if( isWireframe )
			Gizmos.DrawWireSphere(transform.position, radius);
		else
			Gizmos.DrawSphere(transform.position, radius);
	}
}

#endif