using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ExitGates : MonoBehaviour {

	public bool exit;
	public GameObject partMention;
	public GameObject receptionist;
	public GameObject floor;
	public GameObject labEntrance;
	public Transform spawn;

	private float time;
	private bool partDone;
	private bool partShown;
	private GameObject directionPanel;

	public void Start() {
		exit = false;
		partDone = false;
		partShown = false;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(1).gameObject;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (exit) {
			Exit ();
			exit = false;
		}
	}

	private void Exit() {
		directionPanel.SetActive (false);
		transform.GetComponent<SpecifyMovementScript> ().repeating = false;
		Debug.Log ("mentioning while exit");
		partMention.SetActive (true);
		partMention.transform.GetChild(0).GetComponent<Text> ().text = "congrats";
		partMention.transform.GetChild(1).GetComponent<Text> ().text = "Part 1 complete";
		partDone = true;
		time = Time.time;
	}

	public void Update() {
		if (partDone && Time.time - time > 3) {
			time = Time.time;
			partDone = false;
			partShown = true;
			partMention.transform.GetChild (0).GetComponent<Text> ().text = "part 2";
			partMention.transform.GetChild (1).GetComponent<Text> ().text = "first week";
		} else if (partShown && Time.time - time > 3) {
			time = Time.time;
			partShown = false;
			partMention.SetActive (false);
			GameObject.FindGameObjectWithTag ("Player").transform.position = spawn.position;
			receptionist.GetComponent<SpecifyMovementScript> ().ChangeRepeatingText ("receptionist: You can find labs through the doors on my right!!");
			transform.GetComponent<SpecifyMovementScript> ().repeating = true;
			WelcomeScript ();
		}
	}

	private void WelcomeScript() {
		transform.GetComponent<SpecifyMovementScript> ().closePanelOnExit = false;
		transform.GetComponent<SpecifyMovementScript> ().dateTime = Time.time;
		floor.transform.GetComponent<Huxely_2ndFloor> ().index = 0;
		string[] text = new string[floor.transform.GetComponent<Huxely_2ndFloor> ().maxLen];
		text [0] = "Welcome back               Congratulations                now you are a student here";
		text [1] = "Today you need to submit            your first assignment              Lets do it together";
		text [2] = "You need to submit it both electronically and physically";
		text [3] = "Go to computing labs and loging to one of the computers";
		floor.transform.GetComponent<Huxely_2ndFloor> ().text = text;
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text [0]);
		floor.transform.GetComponent<Huxely_2ndFloor> ().dateTime = DateTime.Now;
		directionPanel.SetActive (true);
		labEntrance.transform.GetComponent<LabsEntrance> ().student = true;
	}
}
