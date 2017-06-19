using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCollider : MonoBehaviour {

	public GameObject directionPanel;
	public string text;

	private bool inside;
	private bool firstContact1;
	private bool firstContact2;
	private float dateTime;

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	public void Start() {
		dateTime = Time.time;
		inside = false;
		firstContact1 = true;
		firstContact2 = false;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(1).gameObject;
	}

	public void Update() {
		if (inside && firstContact1) {
			firstContact1 = false;
			firstContact2 = true;
			dateTime = Time.time;
			directionPanel.SetActive (true);
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text);
		} 
		if (firstContact2 && Time.time - dateTime > 3) {
			firstContact2 = false;
			directionPanel.SetActive (false);
		}
	}
}
