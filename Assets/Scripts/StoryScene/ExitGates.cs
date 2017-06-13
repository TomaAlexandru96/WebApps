using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGates : MonoBehaviour {

	public bool exit;

	public void Start() {
		exit = false;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Exit ();
	}

	private void Exit() {
		
	}
}
