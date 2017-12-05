using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance = null;
	private GameManager gameMgr;

	public AudioSource musicSource;

	void Awake() {
		if( instance == null )
			instance = this;
		else if( instance != null )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		gameMgr = GameManager.instance;
	}

	void Update() {
		if( gameMgr.doingSetup )
			return;

		if( !gameMgr.paused ) {
			if( !musicSource.isPlaying ) {
				musicSource.Play();
			}
		} else if( musicSource.isPlaying ) {
			musicSource.Stop();
		}
	}
}
