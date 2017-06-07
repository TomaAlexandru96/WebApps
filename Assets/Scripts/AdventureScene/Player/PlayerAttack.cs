using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	void OnTriggerStay2D (Collider2D coll) {
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (coll.transform.tag == "Enemy") {
				coll.transform.GetComponent<Enemy> ().GetHit (GetComponentInParent<Player> ());
			}	
		}
	}
}
