using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchInterview : MonoBehaviour {

	public GameObject interviewPanel;
	public GameObject currentMap;

	public void Start () {
		interviewPanel.SetActive (false);
	}
	void OnTriggerEnter2D(Collider2D coll) {
		interviewPanel.SetActive (true);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().SetMovement (false);
		GameObject.FindGameObjectWithTag ("Canvas").GetComponent<CanvasScript> ().RemovePartyPanel ();
		GameObject.FindGameObjectWithTag ("Labs").GetComponent<Labs> ().RemoveStudents ();
	}
}
