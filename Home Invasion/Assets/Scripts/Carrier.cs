using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour {

	private GameManager gameMgr;
	private Item item;
	private float timer = 0f;

	public float reach;
	public bool hasItem;
	public float pickUpTime;
	public bool pickingUp = false;

	public void Start() {
		gameMgr = GameManager.instance;
	}

	public void PickUp(Item newItem) {
		if( !hasItem ) {
			item = newItem;

			item.obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			item.obj.GetComponent<BoxCollider2D>().isTrigger = true;

			//print("Picked up " + item.name);

			gameMgr.items.Remove(item);
			gameMgr.pickedItems.Add(item);
			//gameMgr.PrintItems();

			pickingUp = true;
			hasItem = true;
		}
	}

	public void Drop() {
		item.obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		item.obj.GetComponent<BoxCollider2D>().isTrigger = false;

		gameMgr.items.Add(item);
		gameMgr.pickedItems.Remove(item);
		gameMgr.SortItems();

		item = null;
		hasItem = false;
	}

	public void RemoveItemFromScene() {
		Destroy(item.uiObj);
		Destroy(item.obj);
	}

	public Item.ItemType GetItemType() {
		return hasItem ? item.type : Item.ItemType.None;
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
			item.obj.transform.SetPositionAndRotation(new Vector3(x, y, 0), Quaternion.identity);
		} else if( hasItem ) {
			item.obj.transform.SetPositionAndRotation(transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
		}
	}
}
