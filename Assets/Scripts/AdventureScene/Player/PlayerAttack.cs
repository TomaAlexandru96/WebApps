using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private bool canAttack = false;

	public void StartAttack () {
		canAttack = true;
	}

	public void StopAttack () {
		canAttack = false;
	}

	void OnTriggerStay2D (Collider2D col) {
		if (canAttack) {
			if (col.transform.tag == "Enemy") {
				col.transform.GetComponent<Enemy> ().GetHit (GetComponentInParent<Player> ());
			}
		}
	}
}
