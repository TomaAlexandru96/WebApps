using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecifyMovementScript : MonoBehaviour {


	public GameObject SpecifyMovementPanel;

	public void Start () {
		SpecifyMovementPanel.SetActive (false);

	}

	void OnTriggerEnter2D(Collider2D coll) {
		SpecifyMovementPanel.SetActive (true);
	}

	void OnTriggerExit2D(Collider2D coll) {
		SpecifyMovementPanel.SetActive (false);

	}


	public void Update () {

	}
}
