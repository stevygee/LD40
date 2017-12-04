using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollList : MonoBehaviour {

	public List<Item> itemList;
	public Transform contentPanel;
	public SimpleObjectPool buttonObjectPool;
	
	void Start() {
		RefreshDisplay();
	}

	public void RefreshDisplay() {
		AddButtons();
	}

	private void AddButtons() {
		for( int i = 0; i < itemList.Count; i++ ) {
			Item item = itemList[i];
			GameObject newButton = buttonObjectPool.GetObject();
			newButton.transform.SetParent(contentPanel);

			ItemButton itemButton = newButton.GetComponent<ItemButton>();
			itemButton.Setup(item, this);
		}
	}
}
