using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : IComparable<Item> {
	public enum ItemType {
		None,
		Safe,
		Furniture,
		Car,
		Security
	}

	public GameObject obj;
	public GameObject uiObj;

	public string name;
	public float value;
	public bool sellable;
	public ItemType type;

	public Item(GameObject obj, GameObject uiObj, string name, float value, bool sellable, ItemType type) {
		this.obj = obj;
		this.uiObj = uiObj;
		this.name = name;
		this.value = value;
		this.sellable = sellable;
		this.type = type;
	}
	
	public int CompareTo(Item other) {
		if( other == null ) {
			return 1;
		}

		// Return the difference in value
		return (int)(value - other.value);
	}

	public Item Copy() {
		Item copy = new Item(this.obj, this.uiObj, this.name, this.value, this.sellable, this.type);
		return copy;
	}
}
