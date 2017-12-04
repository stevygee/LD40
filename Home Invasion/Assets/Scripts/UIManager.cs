using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	private GameManager gameMgr;

	private Text marketWorthText; 
	public GameObject valueText;

	private GameObject shopPanel;
	private Button openShopPanelButton;
	private Button closeShopPanelButton;

	private GameObject canvas;
	private GameObject gameOverPanel;
	private Button restartButton;

	public List<Item> shopItems;

	private Item activeItem;
	
	private void Awake() {
		if( instance == null )
			instance = this;
		else if( instance != this )
			Destroy(gameObject);

		gameMgr = gameObject.GetComponent<GameManager>();
	}

	public void Init() {
		canvas = GameObject.Find("Canvas");

		activeItem = null;

		// Always visible
		marketWorthText = GameObject.Find("MarketWorthText").GetComponent<Text>();
		openShopPanelButton = GameObject.Find("OpenShopPanelButton").GetComponent<Button>();
		openShopPanelButton.onClick.AddListener(OpenShop);

		// Shop panel
		shopPanel = GameObject.Find("ShopPanel");
		closeShopPanelButton = GameObject.Find("ShopPanel/CloseButton").GetComponent<Button>();
		closeShopPanelButton.onClick.AddListener(CloseShop);
		shopPanel.SetActive(false);

		// Game over panel
		gameOverPanel = GameObject.Find("GameOverPanel");
		restartButton = GameObject.Find("GameOverPanel/RestartButton").GetComponent<Button>();
		restartButton.onClick.AddListener(RestartButton);
		gameOverPanel.SetActive(false);
	}

	public void OpenShop() {
		shopPanel.SetActive(true);
		openShopPanelButton.interactable = false;
	}

	public void CloseShop() {
		shopPanel.SetActive(false);
		openShopPanelButton.interactable = true;
	}

	public void OpenGameOverPanel() {
		gameOverPanel.SetActive(true);
	}

	private void RestartButton() {
		gameMgr.Restart();
	}

	public GameObject AddValueUI() {
		// Value UI
		GameObject textInstance = Instantiate(valueText, new Vector3(0, 0, 0), Quaternion.identity);
		textInstance.transform.SetParent(canvas.transform);
		return textInstance;
	}

	public void SetActiveItem(Item item) {
		activeItem = item;
	}

	public Item GetActiveItem() {
		return (activeItem == null) ? null : activeItem.Copy();
	}

	void Update() {
		if( gameMgr.doingSetup )
			return;

		marketWorthText.text = "Total market worth: $ " + gameMgr.marketWorth;

		// Update both picked up and untouched items
		UpdateValueUIs(gameMgr.items);
		UpdateValueUIs(gameMgr.pickedItems);
	}

	void UpdateValueUIs(List<Item> items) {
		foreach( Item item in items ) {
			if( item.obj == null || item.uiObj == null )
				continue;

			// Set value
			item.uiObj.GetComponent<Text>().text = "$ " + item.value;

			// Set position
			Vector3 itemPos = Camera.main.WorldToScreenPoint(item.obj.GetComponent<Transform>().position);
			item.uiObj.GetComponent<RectTransform>().position = itemPos + (Vector3.up * 28);
		}
	}
}
