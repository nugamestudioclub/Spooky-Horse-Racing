using UnityEngine;

public class RacePlayerHitbox : MonoBehaviour {
	[SerializeField]
	private RacePlayer racer;

	void OnTriggerEnter2D(Collider2D other) {
		if( other.CompareTag("Midpoint") ) {
			racer.HasReachedMidpoint = true;
		}
		if( racer.HasReachedMidpoint && other.CompareTag("FinishLine") ) {
			racer.HasReachedFinishLine = true;
			racer.ControlEnabled = false;
		}
	}
}