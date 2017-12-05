using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private UIManager uiMgr;

	public bool paused;
	public bool firstLaunch = true;

	public float marketWorth;
	public Item safePrefab;
	private Item safe;

	public int minSalary;
	public int maxSalary;
	public float moneyRate;
	private float moneyTimer;
	
	private GameObject[] spawnZones;
	public GameObject thiefPrefab;
	public float spawnRate;
	private float spawnTimer;

	public GameObject tempItem;
	public bool doingSetup;
	public List<Item> items;
	public List<Item> pickedItems;

	private Transform itemsHolder;
	private Transform thievesHolder;

	private bool gameOver;

	private void Awake() {
		if( instance == null )
			instance = this;
		else if( instance != this )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		uiMgr = GetComponent<UIManager>();
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		InitGame();
	}

	void InitGame() {
		doingSetup = true;

		marketWorth = 0;
		moneyTimer = 0f;
		spawnTimer = 0f;
		gameOver = false;
		items = new List<Item>();
		pickedItems = new List<Item>();

		itemsHolder = new GameObject("Items").transform;
		thievesHolder = new GameObject("Thieves").transform;

		// UI
		uiMgr.Init();

		// Add initial items
		safe = AddItem(safePrefab, new Vector3(0, 1, 0), 1f);

		//PrintItems();

		// Spawn
		spawnTimer = spawnRate;
		spawnZones = GameObject.FindGameObjectsWithTag("Respawn");

		doingSetup = false;
	}

	public void PrintItems() {
		print("Items:");
		print("---");
		foreach( Item item in items ) {
			print(item.name + " " + item.value);
		}
		print("---");
	}

	public Item AddItem(Item prefab, Vector3 position, float valueModifier) {
		Item item = prefab.Copy(); // Make a copy before modifying
		item.value = item.value * valueModifier; // Convert store price to market value after buying e.g. loss of value

		item.obj = Instantiate(item.obj, position, Quaternion.identity);
		item.obj.transform.SetParent(itemsHolder);

		item.uiObj = uiMgr.AddValueUI();
		
		items.Add(item);
		SortItems();

		CalculateMarketWorth();

		return item;
	}

	public void SortItems() {
		items.Sort();
		items.Reverse();
	}

	public Item GetMostValuableItem() {
		if( items.Count > 0 ) {
			return items[0];
		} else {
			return null;
		}
	}

	public Vector3 GetClosestRelativeSpawnPosition(Vector3 position) {
		float shortestDistance = -1;
		Vector3 closestRelativePos = new Vector3(-99, 0, 0);
		foreach( GameObject spawnZone in spawnZones ) {
			Vector3 relativePos = spawnZone.transform.InverseTransformPoint(position);
			float distance = relativePos.magnitude;
			if( distance < shortestDistance || distance == -1 ) {
				shortestDistance = distance;
				closestRelativePos = relativePos;
			}
		}
		return closestRelativePos;
	}

	public void RemoveItemFromScene(Item item) {
		Destroy(item.uiObj);
		Destroy(item.obj);
		items.Remove(item);
		pickedItems.Remove(item);

		CalculateMarketWorth();
	}

	private void CalculateMarketWorth() {
		marketWorth = 0;
		foreach( Item item in items ) {
			marketWorth += item.value;
		}
	}

	public bool SpendMoney(float amount) {
		if( amount <= safe.value ) {
			safe.value -= amount;
			return true;
		}

		return false;
	}

	public void ReceiveMoney(float amount) {
		safe.value += amount;
	}

	void Update() {
		if( doingSetup || paused )
			return;

		// Make money
		moneyTimer += Time.deltaTime;
		if( moneyTimer >= moneyRate ) {
			moneyTimer = 0f;
			float salary = Random.Range(minSalary, maxSalary);
			ReceiveMoney(salary);
			CalculateMarketWorth();
		}

		// Spawn rate
		float min = 1;
		float max = 40;
		float log = Mathf.Log(marketWorth, max);
		log = (log <= 0) ? 1 : log;
		spawnRate = (max / log) + min - 1;

		// Spawn thieves
		spawnTimer += Time.deltaTime;
		if( spawnTimer >= spawnRate ) {
			spawnTimer = 0f;

			// Select spawn zone
			int randomIndex = Random.Range(0, spawnZones.Length);
			GameObject spawnZone = spawnZones[randomIndex];
			Vector3 spawnPosition = spawnZone.transform.position;

			// Spawn thief
			GameObject instance = Instantiate(thiefPrefab, spawnPosition, Quaternion.identity);
			instance.transform.SetParent(thievesHolder);
		}
		
		// Add item
		bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();
		Item newItem = uiMgr.GetActiveItem();

		if( newItem != null && Input.GetMouseButtonDown(0) && !mouseOverUI && !gameOver ) {
			// Mouse position to world position
			Camera cam = Camera.main;
			Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
			mouseWorldPos.z = 0f;

			if( SpendMoney(newItem.value) ) {
				AddItem(newItem, mouseWorldPos, 0.5f);
			}
		}
	}

	public void GameOver() {
		gameOver = true;
		uiMgr.CloseShop();
		uiMgr.OpenGameOverPanel();
	}

	public void Restart() {
		int i = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(i);
	}
}
