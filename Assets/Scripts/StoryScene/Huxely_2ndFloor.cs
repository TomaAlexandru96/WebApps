using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Huxely_2ndFloor : MonoBehaviour {

	private GameObject directionPanel;
	private int maxLen = 4;
	public string[] text;
	private int index; 
	private DateTime dateTime;

	public void Start () {
		text = new string[maxLen];
		text [0] = "Welcome to               Imperial College London...        (press space key to continue)";
		text [1] = "Today is your interview day!            Hope you are not too nervous ";
		text [2] = "This to Huxley!               Most of your time in imperial will be spend here...";
		text [3] = "Go And Talk to receptionist to find out about the next steps.";
		directionPanel = GameObject.FindGameObjectsWithTag ("DirectionPanel")[0];
		directionPanel.SetActive (true);
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (this.text[0]);
		index = 1;
		dateTime = DateTime.MinValue;
	}

	public void Update() {
		if (Input.anyKey && index < maxLen && (DateTime.Now - dateTime).Seconds > 1){
			Debug.Log ("Diplay text please");
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (this.text[index]);
			dateTime = DateTime.Now;
			index++;
		}
		if (index >= maxLen && (DateTime.Now - dateTime).Seconds > 1) {
			Debug.Log ("Hahahah");
			directionPanel.SetActive (false);
			dateTime = DateTime.MaxValue;
		}
	}
}
