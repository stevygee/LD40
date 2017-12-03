using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IComparable<Item> {
	public string name;
	public int value;

	private GameObject obj;
	private bool initialized = false;

	public Item(GameObject obj, string name, int value) {
		this.obj = obj;
		this.name = name;
		this.value = value;
	}
	
	public int CompareTo(Item other) {
		if( other == null ) {
			return 1;
		}

		// Return the difference in value
		return value - other.value;
	}
}
