using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabsEntrance : MonoBehaviour {

	public GameObject directionPanel;
	public string repeatingText;
	public bool repeating;
	public bool student;
	private bool inside;
	private bool closeOnce;

	public GameObject thisScene;
	public GameObject nextScene;
	public Transform spawn;

	public void Start () {
		student = false;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(1).gameObject;
		closeOnce = false;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (false);
		inside = false;
		closeOnce = true;
	}

	public void Update() {
		if (inside && !student) {
			directionPanel.SetActive (true);
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (repeatingText);
		} else if (inside && student) {
			thisScene.SetActive (false);
			nextScene.SetActive (true);
			GameObject.FindGameObjectWithTag ("Player").transform.position = spawn.position;
		} else if (closeOnce){
			directionPanel.SetActive (false);
			closeOnce = false;
		}
	}
}
