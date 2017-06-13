using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
		partMention.SetActive (true);
		partMention.transform.GetChild(0).GetComponent<Text> ().text = "congratulations";
		partMention.transform.GetChild(1).GetComponent<Text> ().text = "Part 1 complete";
	}
}
