﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance = null;

	void Awake() {
		if( instance == null )
			instance = this;
		else if( instance != null )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}
}
