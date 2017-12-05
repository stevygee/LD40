using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour {
	private GameManager gameMgr;
	private Rigidbody2D rb;
	private Animator alarmState;

	public float duration;
	private float timer;

	private int activeStateHash = Animator.StringToHash("Base Layer.Active");
	private int inactiveStateHash = Animator.StringToHash("Base Layer.Inactive");
	private int triggeredStateHash = Animator.StringToHash("Base Layer.Triggered");

	private int thiefInRangeHash = Animator.StringToHash("thiefInRange");
	private int turnOnHash = Animator.StringToHash("turnOn");
	private int timeOutHash = Animator.StringToHash("timeOut");

	void Start() {
		gameMgr = GameManager.instance;
		rb = GetComponent<Rigidbody2D>();
		alarmState = GetComponent<Animator>();
	}

	private bool IsAlarmState(int hash) {
		AnimatorStateInfo stateInfo = alarmState.GetCurrentAnimatorStateInfo(0);
		return (stateInfo.fullPathHash == hash);
	}

	void Update() {
		if( gameMgr.doingSetup || gameMgr.paused )
			return;

		// State changes
		// ---

		// State specific behaviour
		// ---

		if( IsAlarmState(triggeredStateHash) ) {
			timer += Time.deltaTime;
			if( timer >= duration ) {
				timer = 0f;
				alarmState.SetBool(timeOutHash, true);
			}
		}

		if( IsAlarmState(inactiveStateHash) ) {
			alarmState.SetBool(thiefInRangeHash, false);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		print("trigger" + other);
		// Alarm gets triggered by nearby thieves
		if( IsAlarmState(activeStateHash) && other.gameObject.tag == "Thief" ) {
			alarmState.SetBool(thiefInRangeHash, true);
		}
	}
}
