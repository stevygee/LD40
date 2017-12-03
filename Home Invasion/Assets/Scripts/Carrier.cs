using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour {

	GameObject item;

	void PickUp(GameObject newItem) {
		if( !item ) {
			item = newItem;
		}
	}

	void Drop() {
		item = null;
	}
}
