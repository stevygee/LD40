using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGetItem : MonoBehaviour {

	bool active;
	Carrier carrier;

	void Start() {
		carrier = gameObject.GetComponent<Carrier>();
	}

	void Update() {
		if( !active )
			return;

		// Closest and most valuable
		// select from global array
		//wantedItem = ...

		if( false ) { // within reach
			//carrier.PickUp(wantedItem);
		} else {
			// move towards item
		}
	}
}
