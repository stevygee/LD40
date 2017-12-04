using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	private GameManager gameMgr;

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
}
