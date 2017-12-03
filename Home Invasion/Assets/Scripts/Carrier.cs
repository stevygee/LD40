using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour {

	private GameObject item;
	private bool pickingUp = false;

	public float reach;
	public bool hasItem;

	public void PickUp(GameObject newItem) {
		if( !hasItem && !pickingUp ) {
			item = newItem;
			// TODO: Timer

			item.transform.SetParent(gameObject.transform);

			pickingUp = true;
			hasItem = true;
		}
	}

	public void Drop() {
		item = null;
	}

	public void Update() {
		if( hasItem ) {
			item.transform.localPosition = new Vector3(0, 1.5f, 0);
			item.transform.localRotation = Quaternion.identity;
		}
	}
}
