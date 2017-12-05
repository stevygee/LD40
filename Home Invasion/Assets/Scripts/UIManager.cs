using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	private GameManager gameMgr;

	private Text marketWorthText;
	public GameObject valueText;

	private GameObject introPanel;
	private Button introContinueButton;

	private GameObject menuPanel;
	private Button startGameButton;

	private GameObject shopPanel;
	private Button openShopPanelButton;
	private Button closeShopPanelButton;

	private Transform valuesContainer;
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
		valuesContainer = GameObject.Find("Canvas/Values").GetComponent<Transform>();

		activeItem = null;

		// Always visible
		marketWorthText = GameObject.Find("MarketWorthText").GetComponent<Text>();
		openShopPanelButton = GameObject.Find("OpenShopPanelButton").GetComponent<Button>();
		openShopPanelButton.onClick.AddListener(OpenShop);

		// Menu panel
		menuPanel = GameObject.Find("MenuPanel");
		startGameButton = GameObject.Find("MenuPanel/StartButton").GetComponent<Button>();
		startGameButton.onClick.AddListener(CloseMenu);
		if( !gameMgr.firstLaunch )
			menuPanel.SetActive(false);

		// Intro panel
		introPanel = GameObject.Find("IntroPanel");
		introContinueButton = GameObject.Find("IntroPanel/ContinueButton").GetComponent<Button>();
		introContinueButton.onClick.AddListener(IntroContinue);
		if( !gameMgr.firstLaunch )
			introPanel.SetActive(false);

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

	public void CloseMenu() {
		menuPanel.SetActive(false);
		gameMgr.firstLaunch = false;
	}

	public void IntroContinue() {
		introPanel.SetActive(false);
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
		textInstance.transform.SetParent(valuesContainer);
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

		gameMgr.paused = menuPanel.activeInHierarchy || introPanel.activeInHierarchy;

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
			Vector3 worldPos = item.obj.GetComponent<Transform>().position + (Vector3.up * 0.75f);
			Vector3 itemPos = Camera.main.WorldToScreenPoint(worldPos);
			item.uiObj.GetComponent<RectTransform>().position = itemPos;
		}
	}
}
