using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGetItem : MonoBehaviour {

	private Carrier carrier;
	private GameManager gameMgr;
	private Rigidbody2D rb;

	public bool active;
	public float speed;

	void Start() {
		gameMgr = GameManager.instance;
		carrier = gameObject.GetComponent<Carrier>();
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		if( !active )
			return;

		// Closest and most valuable item
		Item wantedItem = gameMgr.GetMostValuableItem();
		if( wantedItem == null )
			return;

		Vector3 relativePos = wantedItem.obj.transform.InverseTransformPoint(transform.position);

		if( relativePos.magnitude <= carrier.reach ) { // within reach
			carrier.PickUp(wantedItem.obj);
		} else {
			// Move towards item
			float moveHorizontal = (relativePos.x > 0) ? -1 : 1;
			Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);

			rb.AddForce(movement * speed);
		}
	}
}
