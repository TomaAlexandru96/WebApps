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
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (false);
		inside = false;
	}


	public void Update () {
		if (inside && index < maxSize &&  (DateTime.Now - dateTime).Seconds > 1) {
			Debug.Log("printitng + " + text [index]);
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text[index]);
			index++;
		}
		if (index >= maxSize && (DateTime.Now - dateTime).Seconds > 1) {
			directionPanel.SetActive (false);
			dateTime = DateTime.MaxValue;
		}
	}
}
