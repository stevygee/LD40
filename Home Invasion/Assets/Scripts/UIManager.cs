using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	private GameManager gameMgr;

	public GameObject valueText;

	private GameObject canvas;
	private GameObject gameOverPanel;
	private Button restartButton;

	private void Awake() {
		if( instance == null )
			instance = this;
		else if( instance != this )
			Destroy(gameObject);

		gameMgr = gameObject.GetComponent<GameManager>();
	}

	public void Init() {
		canvas = GameObject.Find("Canvas");

		// Game over panel
		gameOverPanel = GameObject.Find("GameOverPanel");
		restartButton = GameObject.Find("GameOverPanel/RestartButton").GetComponent<Button>();
		restartButton.onClick.AddListener(RestartButtonClick);
		gameOverPanel.SetActive(false);
	}

	public void OpenGameOverPanel() {
		gameOverPanel.SetActive(true);
	}

	private void RestartButtonClick() {
		gameMgr.Restart();
	}

	public GameObject AddValueUI() {
		// Value UI
		GameObject textInstance = Instantiate(valueText, new Vector3(0, 0, 0), Quaternion.identity);
		textInstance.transform.SetParent(canvas.transform);
		return textInstance;
	}

	void Update() {
		if( gameMgr.doingSetup )
			return;

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
