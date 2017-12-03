using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour {

	private GameObject item;
	private float timer = 0f;

	public float reach;
	public bool hasItem;
	public float pickUpTime;
	public bool pickingUp = false;

	public void PickUp(GameObject newItem) {
		if( !hasItem && !pickingUp ) {
			item = newItem;
			// TODO: Timer

			item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			//item.GetComponent<BoxCollider2D>().isTrigger = true;

			pickingUp = true;
			hasItem = true;
		}
	}

	public void Drop() {
		item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		item = null;
		hasItem = false;
	}

	public void Update() {
		if( timer >= pickUpTime ) {
			timer = 0f;
			pickingUp = false;
		}

		if( pickingUp ) {
			timer += Time.deltaTime;

			float radius = 1.25f;
			float angle = timer / pickUpTime * Mathf.PI * 2 * 0.25f;
			float x = transform.position.x + Mathf.Cos(angle) * radius;
			float y = transform.position.y + Mathf.Sin(angle) * radius;
			item.transform.SetPositionAndRotation(new Vector3(x, y, 0), Quaternion.identity);
		} else if( hasItem ) {
			item.transform.SetPositionAndRotation(transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
		}
	}
}
