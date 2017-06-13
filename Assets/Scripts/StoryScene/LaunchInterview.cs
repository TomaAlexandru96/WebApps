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
	}
}
