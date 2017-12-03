using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private GameObject[] spawnZones;

	public GameObject thiefPrefab;
	public float spawnRate;
	private float spawnTimer = 0f;

	public GameObject tempItem;
	public bool doingSetup;
	public List<Item> items;

	private Transform itemsHolder;
	private Transform thievesHolder;

	private void Awake() {
		if( instance == null )
			instance = this;
		else if( instance != this )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
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

		items = new List<Item>();
		itemsHolder = new GameObject("Items").transform;
		thievesHolder = new GameObject("Thieves").transform;

		// Add initial items
		AddItem(tempItem, new Vector3(3, 2, 0), "Couch", 200);
		AddItem(tempItem, new Vector3(2, 1, 0), "Car", 500000);
		AddItem(tempItem, new Vector3(5, 4, 0), "TV", 5000);

		PrintItems();

		// Spawn zones
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

	public void AddItem(GameObject prefab, Vector3 position, string name, int value) {
		GameObject instance = Instantiate(prefab, position, Quaternion.identity);
		instance.transform.SetParent(itemsHolder);

		Item newItem = new Item(instance, name, value);

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

		spawnTimer += Time.deltaTime;
		if( spawnTimer >= spawnRate ) {
			spawnTimer = 0f;

			// Select spawn zone
			int randomIndex = Random.Range(0, spawnZones.Length);
			GameObject spawnZone = spawnZones[randomIndex];
			Vector3 spawnPosition = spawnZone.transform.position;

			// Spawn thief
			Instantiate(thiefPrefab, spawnPosition, Quaternion.identity);
		}
	}
}
