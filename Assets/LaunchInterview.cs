using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchInterview : MonoBehaviour {

	public GameObject interviewPanel;

	public void Start () {
		interviewPanel.SetActive (false);
	}
	void OnTriggerEnter2D(Collider2D coll) {
		interviewPanel.SetActive (true);
	}

	void OnTriggerExit2D(Collider2D coll) {
		interviewPanel.SetActive (true);}
}
