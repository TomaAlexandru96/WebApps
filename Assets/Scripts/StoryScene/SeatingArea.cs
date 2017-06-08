using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingArea : MonoBehaviour {


	private Transform toni;

	void Start() {
		toni = GameObject.FindGameObjectWithTag ("ToniCollider").transform;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		toni.GetComponent<ToniScript> ().seated = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		toni.GetComponent<ToniScript> ().seated = false;
	}
}
