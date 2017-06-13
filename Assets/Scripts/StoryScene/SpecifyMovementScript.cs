using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecifyMovementScript : MonoBehaviour {


	public GameObject directionPanel;
	public int maxSize;
	public string[] text;
	public string repeatingText;
	public string repeatingText2;
	public int index = 0;
	public bool repeating;
	public bool repeating2;
	protected bool firstContact;
	protected bool inside;
	protected float dateTime;

	public void Start () {
		dateTime = Time.time -1;
		firstContact = true;
		repeating2 = false;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(1).gameObject;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		directionPanel.SetActive (false);
		inside = false;
	}


	protected virtual void ExtendFunction() {}

	protected virtual void Conversation() {
		directionPanel.SetActive (true);
		dateTime = Time.time;
		directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (text[index]);
		index++;
		firstContact = false;
	}

	public void Update () {
		ExtendFunction ();
		if ((Input.GetKeyDown("space") || firstContact) && inside && index < maxSize &&  (Time.time - dateTime) > 0.5) {
			Conversation();
		}
		if (inside && index >= maxSize && (Time.time - dateTime) > 1) {
			if (repeating2) {
				directionPanel.SetActive (true);
				directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (repeatingText2);
				repeating = false;
			} else if (repeating) {
				directionPanel.SetActive (true);
				directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (repeatingText);
			} else {
				directionPanel.SetActive (false);
			}
		}
	}
}
