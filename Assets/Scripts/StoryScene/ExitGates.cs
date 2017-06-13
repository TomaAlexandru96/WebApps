using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGates : MonoBehaviour {

	public bool exit;
	public GameObject partMention;
	public GameObject floor;


	public void Start() {
		exit = false;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (exit) {
			Exit ();
		}
	}

	private void Exit() {
		floor.SetActive (false);
	}
}
