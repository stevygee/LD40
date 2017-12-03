using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject tempItem;
	public bool doingSetup;
	public List<Item> items;

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

		// Add initial items
		AddItem(tempItem, new Vector3(3, 2, 0), "Couch", 200);
		AddItem(tempItem, new Vector3(2, 1, 0), "Car", 500000);
		AddItem(tempItem, new Vector3(5, 4, 0), "TV", 5000);

		foreach( Item item in items ) {
			print(item.name + " " + item.value);
		}

		doingSetup = false;
	}

	public void AddItem(GameObject prefab, Vector3 position, string name, int value) {
		GameObject obj = Instantiate(prefab, position, Quaternion.identity);
		Item newItem = new Item(obj, name, value);

		items.Add(newItem);
		items.Sort();
		items.Reverse();
	}

	void Update() {
		if( doingSetup )
			return;

		// ...
	}
}
