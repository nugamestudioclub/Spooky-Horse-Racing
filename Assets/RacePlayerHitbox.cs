using UnityEngine;

public class RacePlayerHitbox : MonoBehaviour {
	[SerializeField]
	private RacePlayer racer;

	void OnTriggerEnter2D(Collider2D other) {
		if( other.CompareTag("Midpoint") ) {
			racer.HasReachedMidpoint = true;
            print($"{racer.Name} has reached the midpoint!");

        }
        if ( racer.HasReachedMidpoint && other.CompareTag("FinishLine") ) {
			racer.HasReachedFinishLine = true;
			racer.ControlEnabled = false;
			print($"{racer.Name} has reached the midpoint!");
		}
	}
}