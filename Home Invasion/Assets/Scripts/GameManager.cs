using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public bool doingSetup;

	private void Awake() {
		if( instance == null )
			instance = this;
		else if( instance != this )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}


	void InitGame() {
		doingSetup = true;

		// ...

		doingSetup = false;
	}

	void Update () {
		if( doingSetup )
			return;

		// ...
	}
}
