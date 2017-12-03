using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour {

	private GameObject item;

	public float reach;

	public void PickUp(GameObject newItem) {
		if( !item ) {
			item = newItem;
			print("Picked up item!");
		}
	}

	public void Drop() {
		item = null;
	}
}
