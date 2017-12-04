using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private UIManager uiMgr;

	private GameObject[] spawnZones;

	public GameObject thiefPrefab;
	public float spawnRate;
	private float spawnTimer;

	public GameObject tempItem;
	public bool doingSetup;
	public List<Item> items;

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
		
		spawnTimer = 0f;
		gameOver = false;
		items = new List<Item>();

		itemsHolder = new GameObject("Items").transform;
		thievesHolder = new GameObject("Thieves").transform;

		// Add initial items
		AddItem(tempItem, new Vector3(0, 1, 0), "Vault", 1000000, false, Item.ItemType.Vault);
		AddItem(tempItem, new Vector3(3, 2, 0), "Couch", 200, true, Item.ItemType.Furniture);
		AddItem(tempItem, new Vector3(2, 1, 0), "Car", 500000, true, Item.ItemType.Car);
		AddItem(tempItem, new Vector3(5, 4, 0), "TV", 5000, true, Item.ItemType.Furniture);

		//PrintItems();

		// Spawn
		spawnTimer = spawnRate;
		spawnZones = GameObject.FindGameObjectsWithTag("Respawn");

		// UI
		uiMgr.Init();

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

	public void AddItem(GameObject prefab, Vector3 position, string name, int value, bool sellable, Item.ItemType type) {
		GameObject instance = Instantiate(prefab, position, Quaternion.identity);
		instance.transform.SetParent(itemsHolder);

		Item newItem = new Item(instance, name, value, sellable, type);

		items.Add(newItem);
		SortItems();
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

	void Update() {
		if( doingSetup )
			return;

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
		if( Input.GetMouseButtonDown(0) ) {
			// Mouse position to world position
			Camera cam = Camera.main;
			Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
			mouseWorldPos.z = 0f;

			AddItem(tempItem, mouseWorldPos, "New Item", 1000000, true, Item.ItemType.Car);
		}
	}

	public void GameOver() {
		gameOver = true;
		uiMgr.OpenGameOverPanel();
	}

	public void Restart() {
		int i = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(i);
	}
}
