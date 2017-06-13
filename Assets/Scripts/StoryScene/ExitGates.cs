using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ExitGates : MonoBehaviour {

	public bool exit;
	public GameObject partMention;
	public GameObject directionPanel;
	public GameObject receptionist;
	public GameObject floor;
	public Transform spawn;

	private float time;
	private bool partDone;
	private bool partShown;

	public void Start() {
		exit = false;
		partDone = false;
		partShown = false;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (exit) {
			Exit ();
			exit = false;
		}
	}

	private void Exit() {
		directionPanel.SetActive (false);
		receptionist.GetComponent<SpecifyMovementScript> ().repeating = false;
		partMention.SetActive (true);
		partMention.transform.GetChild(0).GetComponent<Text> ().text = "congratulations";
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
//			directionPanel.GetComponent<DirectionPanel> ().DisplayText ("Welcome to your first week, you are now a student");
			receptionist.GetComponent<SpecifyMovementScript> ().ChangeRepeatingText ("receptionist: You can find labs through the doors on my right!!");
		}
	}
}
