using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefAI : MonoBehaviour {

	private Carrier carrier;
	private GameManager gameMgr;
	private Rigidbody2D rb;
	private Animator aiState;
	private Vector3 relativeItemPos;

	private int searchingStateHash = Animator.StringToHash("Base Layer.Searching");
	private int pickingUpItemStateHash = Animator.StringToHash("Base Layer.PickingUpItem");
	private int leavingStateHash = Animator.StringToHash("Base Layer.Leaving");
	private int fleeingStateHash = Animator.StringToHash("Base Layer.Fleeing");

	private int itemInRangeHash = Animator.StringToHash("itemInRange");
	private int scaredHash = Animator.StringToHash("scared");
	private int hasItemHash = Animator.StringToHash("hasItem");

	public bool active;
	public float speed;

	void Start() {
		gameMgr = GameManager.instance;
		carrier = gameObject.GetComponent<Carrier>();
		rb = GetComponent<Rigidbody2D>();
		aiState = GetComponent<Animator>();
	}

	private bool IsAIState(int hash) {
		AnimatorStateInfo stateInfo = aiState.GetCurrentAnimatorStateInfo(0);
		return (stateInfo.fullPathHash == hash);
	}

	void Update() {
		if( !active )
			return;

		// Choose closest and most valuable item
		Item wantedItem = gameMgr.GetMostValuableItem();
		if( wantedItem == null )
			return;

		relativeItemPos = wantedItem.obj.transform.InverseTransformPoint(transform.position);

		// State changes
		// ---
		if( relativeItemPos.magnitude <= carrier.reach ) {
			aiState.SetTrigger(itemInRangeHash);
		}

		if( carrier.hasItem && !carrier.pickingUp ) {
			aiState.SetTrigger(hasItemHash);
		}

		// TODO: Scare

		// State specific behaviour
		// ---

		if( IsAIState(pickingUpItemStateHash) ) {
			// Pick up
			carrier.PickUp(wantedItem.obj);
		}
	}

	void FixedUpdate() {
		// State specific behaviour
		// ---
		float moveHorizontal = 0f;

		if( IsAIState(searchingStateHash) ) {
			// Move towards item
			moveHorizontal = (relativeItemPos.x > 0) ? -1 : 1;
		} else if( IsAIState(leavingStateHash) || IsAIState(fleeingStateHash) ) {
			// TODO: Move to closest exit
			moveHorizontal = true ? -1 : 1;
		}

		Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);

		rb.AddForce(movement * speed);
	}
}
