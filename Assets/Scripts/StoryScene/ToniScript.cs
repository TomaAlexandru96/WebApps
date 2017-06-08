using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToniScript : MonoBehaviour {


	public GameObject directionPanel;
	public int maxSize;
	public string[] text;
	public int index = 0;
	public bool seated;
	public bool foodServed;
	public bool foodFinished;
	public bool interview;
	public string foodServingText;
	protected bool firstContact;
	protected bool inside;
	protected float dateTime;
	private Transform huxely308;

	public void Start () {
		dateTime = Time.time -1;
		firstContact = true;
		seated = false;
		foodServed = false;
		foodFinished = false;
		interview = false;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(0).gameObject;
		huxely308 = GameObject.FindGameObjectWithTag ("Huxely308").transform;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (false);
		inside = false;
	}

	protected virtual void Conversation() {
		directionPanel.SetActive (true);
		dateTime = Time.time;
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text[index]);
		index++;
		firstContact = false;
	}

	public void Update () {
		if (!seated && index == 3){
			index = 1;
			dateTime = Time.time;
			return;
		}
		if (inside && index == maxSize && (Time.time - dateTime) > 5) {
			foodServed = true;
			huxely308.GetComponent<Controller308> ().BlankScreen ();
			dateTime = Time.time;
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (foodServingText);
			MakeAllStudentDisappear ();
			index++;
		}
		if (foodServed && (Time.time - dateTime) > 3) {
			//New sudents with position
			MakeAllFoodStudentAppear ();
			dateTime = Time.time;
			foodFinished = true;
			foodServed = false;
			huxely308.GetComponent<Controller308> ().SetFood();
		}
		if (inside && index < maxSize &&  (Time.time - dateTime) > 5) {
			Conversation();
		}
		if (foodFinished && (Time.time - dateTime) > 5) {
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Eating food increase your health, you can grab food with (Space key)");
		}
		if (foodFinished && (Time.time - dateTime) > 15) {
			huxely308.GetComponent<Controller308> ().DissapearFood ();
			FoodGone ();
			foodFinished = false;
			dateTime = Time.time;
			interview = true;
		}
		if (interview && (Time.time - dateTime) > 5) {
			directionPanel.SetActive (true);
			directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText ("Go to 217, 218 for your interview, near main staircase..");
		}
	}

	private void MakeAllStudentDisappear () {
		GameObject[] students = GameObject.FindGameObjectsWithTag ("Student");
		foreach (GameObject student in students) {
			student.SetActive (false);
		}
	}

	private void MakeAllFoodStudentAppear () {
		Transform Huxely308 = GameObject.FindGameObjectWithTag ("Huxely308").transform;
		int maxChild = Huxely308.childCount;
		for (int i = 19; i < maxChild; i++) {
			Huxely308.GetChild (i).gameObject.SetActive (true);
		}
	}

	private void FoodGone() {
		Transform Huxely308 = GameObject.FindGameObjectWithTag ("Huxely308").transform;
		int maxChild = Huxely308.childCount;
		Huxely308.GetChild (maxChild-1).gameObject.SetActive (false);
	}
}
