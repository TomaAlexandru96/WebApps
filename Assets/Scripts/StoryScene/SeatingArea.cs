using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingArea : MonoBehaviour {


	private Transform toni;

	public void Start() {
		toni = GameObject.FindGameObjectWithTag ("ToniCollider").transform;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		toni = GameObject.FindGameObjectWithTag ("ToniCollider").transform;
		toni.GetComponent<ToniScript> ().Seated();
	}

	void OnTriggerExit2D(Collider2D coll) {
		toni.GetComponent<ToniScript> ().seated = false;
	}
}
