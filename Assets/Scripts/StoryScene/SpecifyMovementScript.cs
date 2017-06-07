using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpecifyMovementScript : MonoBehaviour {


	public GameObject directionPanel;
	public int maxSize;
	public string[] text;
	public int index = 0;
	private bool inside;
	private DateTime dateTime;

	public void Start () {
		dateTime = DateTime.MinValue;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(0).gameObject;
//		text[0] = "Receptionist:   Hi There,                       How Can I help You?";
//		text[1] = "uhhh.... I am prospective student, where should I go?";
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (false);
		inside = false;
	}


	public void Update () {
		if (Input.GetKeyDown("space") && inside && index < maxSize &&  (DateTime.Now - dateTime).Seconds > 0.5) {
			Debug.Log("printitng + " + text [index]);
			directionPanel.SetActive (true);
			dateTime = DateTime.Now;
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text[index]);
			index++;
		}
		if (inside && index >= maxSize && (DateTime.Now - dateTime).Seconds > 1) {
			directionPanel.SetActive (false);
			dateTime = DateTime.MaxValue;
		}
	}
}
