using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {
	private UIManager uiMgr;

	public Button button;
	public Text nameLabel;
	public Text priceLabel;
	public Image iconImage;
	private Item item;

	private ShopScrollList scrollList;

	public void Setup(Item item, ShopScrollList scrollList) {
		uiMgr = UIManager.instance;
		this.scrollList = scrollList;

		this.item = item;
		nameLabel.text = item.name;
		priceLabel.text = "$ " + item.value.ToString();
		//iconImage.sprite = item.icon;

		button.onClick.AddListener(OnClick);
	}

	public void OnClick() {
		uiMgr.SetActiveItem(item);
		uiMgr.CloseShop();
	}
}
