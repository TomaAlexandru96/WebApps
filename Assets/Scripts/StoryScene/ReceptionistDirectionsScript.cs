using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionistDirectionsScript : MonoBehaviour {


	public GameObject DirectionsPanel;

	public void Start () {
		DirectionsPanel.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		DirectionsPanel.SetActive (true);
	}

	void OnTriggerExit2D(Collider2D coll) {
		DirectionsPanel.SetActive (false);

	}


	public void Update () {

	}
}
