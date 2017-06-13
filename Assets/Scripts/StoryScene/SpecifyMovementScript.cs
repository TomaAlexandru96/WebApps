using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecifyMovementScript : MonoBehaviour {


	public GameObject directionPanel;
	public int maxSize;
	public string[] text;
	public string repeatingText;
	public int index = 0;
	public bool repeating;
	public bool closePanelOnExit;
	protected bool firstContact;
	protected bool inside;
	public float dateTime;

	public void Start () {
		dateTime = Time.time -1;
		firstContact = true;
		directionPanel = GameObject.FindGameObjectsWithTag ("Canvas")[0].transform.GetChild(1).gameObject;
		closePanelOnExit = true;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		inside = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (closePanelOnExit) {
			directionPanel.SetActive (false);
		}
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
			if (repeating) {
				directionPanel.SetActive (true);
				directionPanel.transform.GetComponent<DirectionPanel> ().DisplayText (repeatingText);
				closePanelOnExit = true;
			} else {
				directionPanel.SetActive (false);
			}
		}
		if ((Time.time - dateTime) > 1) {
			closePanelOnExit = true;
		}
	}

	public void ChangeRepeatingText (string newText) {
		repeating = true;
		repeatingText = newText;
		index = maxSize;
	}
}
