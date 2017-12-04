using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IComparable<Item> {
	public enum ItemType {
		None,
		Vault,
		Furniture,
		Car,
		Security
	}

	public GameObject obj;

	public string name;
	public int value;
	public bool sellable;
	public ItemType type;

	public Item(GameObject obj, string name, int value, bool sellable, ItemType type) {
		this.obj = obj;
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
		return value - other.value;
	}
}
